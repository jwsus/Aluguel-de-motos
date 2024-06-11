using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mottu.Application.Services
{
    public class S3FileService
    {
        private readonly string _awsAccessKey;
        private readonly string _awsSecretKey;
        private readonly string _regionName;
        private readonly string _bucketName;

        public S3FileService(IConfiguration configuration)
        {
            _awsAccessKey = configuration["AWS:AccessKey"];
            _awsSecretKey = configuration["AWS:SecretKey"];
            _regionName = configuration["AWS:Region"];
            _bucketName = configuration["AWS:BucketName"];
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            if (string.IsNullOrEmpty(_awsAccessKey) || string.IsNullOrEmpty(_awsSecretKey) || string.IsNullOrEmpty(_regionName) || string.IsNullOrEmpty(_bucketName))
            {
                throw new Exception("AWS configuration is missing or invalid.");
            }

            var region = RegionEndpoint.GetBySystemName(_regionName);
            var s3Client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, region);

            try
            {
                var fileTransferUtility = new TransferUtility(s3Client);

                await fileTransferUtility.UploadAsync(fileStream, _bucketName, fileName);

                return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
            }
            catch (AmazonS3Exception e)
            {
                throw new Exception($"Error encountered on server. Message:'{e.Message}'", e);
            }
            catch (Exception e)
            {
                throw new Exception($"Unknown error encountered on server. Message:'{e.Message}'", e);
            }
        }
    }
}
