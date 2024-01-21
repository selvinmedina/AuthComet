namespace AuthComet.Mails.Common
{
    public class SmtpSettingsBuilder
    {
        private readonly SmtpSettings _smtpSettings;

        public SmtpSettingsBuilder()
        {
            _smtpSettings = new SmtpSettings();
        }

        public SmtpSettingsBuilder WithHost(string host)
        {
            _smtpSettings.Host = host;
            return this;
        }

        public SmtpSettingsBuilder WithPort(int port)
        {
            _smtpSettings.Port = port;
            return this;
        }

        public SmtpSettingsBuilder WithUsername(string username)
        {
            _smtpSettings.Username = username;
            return this;
        }

        public SmtpSettingsBuilder WithPassword(string password)
        {
            _smtpSettings.Password = password;
            return this;
        }

        public SmtpSettingsBuilder WithEnableSsl(bool enableSsl)
        {
            _smtpSettings.EnableSsl = enableSsl;
            return this;
        }

        public SmtpSettingsBuilder WithFrom(string from)
        {
            _smtpSettings.From = from;
            return this;
        }

        public SmtpSettings Build()
        {
            return _smtpSettings;
        }
    }
}
