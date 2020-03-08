using System.Collections.Generic;
using System.Threading.Tasks;
using Pulumi;
using Aws = Pulumi.Aws;

class Program
{
    static Task Main()
    {
        return Deployment.RunAsync(() =>
        {
            // Create a KMS Key for S3 server-side encryption
            var key = new Aws.Kms.Key("my-key");

            // Create an AWS resource (S3 Bucket)
            var bucket = new Aws.S3.Bucket("my-bucket", new Aws.S3.BucketArgs
            {
                ServerSideEncryptionConfiguration = new Aws.S3.Inputs.BucketServerSideEncryptionConfigurationArgs
                {
                    Rule = new Aws.S3.Inputs.BucketServerSideEncryptionConfigurationRuleArgs
                    {
                        ApplyServerSideEncryptionByDefault = new Aws.S3.Inputs.BucketServerSideEncryptionConfigurationRuleApplyServerSideEncryptionByDefaultArgs
                        {
                            SseAlgorithm = "aws:kms",
                            KmsMasterKeyId = key.Id,
                        },
                    },
                },
            });

            // Export the name of the bucket
            return new Dictionary<string, object> {
                { "bucket_name", bucket.Id },
            };
        });
    }
}