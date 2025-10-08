using Amazon.Runtime;

namespace Bh.AppWeb.Configuracion
{
    public class AwsConfig
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public AWSCredentials GetAwsCredentials()
        {
            return new BasicAWSCredentials(AccessKey, SecretKey);
        }
    }
}
