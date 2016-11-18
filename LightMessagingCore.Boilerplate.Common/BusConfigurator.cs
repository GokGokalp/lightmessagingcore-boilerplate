using System;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace LightMessagingCore.Boilerplate.Common
{
    public class BusConfigurator
    {
        private IRetryPolicy _retryPolicy;
        private int? _tripThreshold;
        private int? _activeThreshold;
        private int? _resetInterval;
        private int? _rateLimit;
        private int? _rateLimiterInterval;


        private static readonly Lazy<BusConfigurator> _Instance = new Lazy<BusConfigurator>(() => new BusConfigurator());

        private BusConfigurator()
        {

        }

        public static BusConfigurator Instance => _Instance.Value;

        public IBusControl ConfigureBus(
    Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(MqConstants.RabbitMQUri), hst =>
                {
                    hst.Username(MqConstants.RabbitMQUserName);
                    hst.Password(MqConstants.RabbitMQPassword);
                });

                UseInnerRetry(cfg);
                UseInnerCircuitBreaker(cfg);
                UseInnerRateLimiter(cfg);

                registrationAction?.Invoke(cfg, host);
            });
        }

        #region Fluent Methods
        public BusConfigurator UseRetry(IRetryPolicy retryPolicy)
        {
            _retryPolicy = retryPolicy;
            return this;
        }

        public BusConfigurator UseCircuitBreaker(int tripThreshold, int activeThreshold, int resetInterval)
        {
            _tripThreshold = tripThreshold;
            _activeThreshold = activeThreshold;
            _resetInterval = resetInterval;

            return this;
        }

        public BusConfigurator UseRateLimiter(int rateLimit, int interval)
        {
            _rateLimit = rateLimit;
            _rateLimiterInterval = interval;

            return this;
        }
        #endregion

        #region Private Methods
        public void UseInnerRetry(IRabbitMqBusFactoryConfigurator cfg)
        {
            if (_retryPolicy != null)
                cfg.UseRetry(_retryPolicy);
        }

        public void UseInnerCircuitBreaker(IRabbitMqBusFactoryConfigurator cfg)
        {
            if (_tripThreshold != null && _activeThreshold != null && _resetInterval != null)
            {
                cfg.UseCircuitBreaker(cb =>
                {
                    cb.TripThreshold = _tripThreshold.Value;
                    cb.ActiveThreshold = _activeThreshold.Value;
                    cb.ResetInterval(TimeSpan.FromMinutes(_resetInterval.Value));
                });
            }
        }

        private void UseInnerRateLimiter(IRabbitMqBusFactoryConfigurator cfg)
        {
            if (_rateLimit != null && _rateLimiterInterval != null)
            {
                cfg.UseRateLimit(_rateLimit.Value, TimeSpan.FromSeconds(_rateLimiterInterval.Value));
            }
        }
        #endregion
    }
}