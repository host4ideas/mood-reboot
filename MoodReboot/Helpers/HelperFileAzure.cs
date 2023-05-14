using MoodReboot.Services;
using NugetMoodReboot.Helpers;

namespace MoodReboot.Helpers
{
    public class HelperFileAzure
    {
        private readonly ServiceStorageBlob serviceStorage;

        public HelperFileAzure(ServiceStorageBlob serviceStorage)
        {
            this.serviceStorage = serviceStorage;
        }

        public Task DeleteFile(int fileId)
        {
            throw new NotImplementedException();
        }

        public bool IsImage(string contentType)
        {
            if (contentType.Contains("image/jpeg") || contentType.Contains("image/png") || contentType.Contains("image/webp"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsExcel(string contentType)
        {
            if (contentType.Contains("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") || contentType.Contains("application/vnd.ms-excel"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsPdf(string contentType)
        {
            if (contentType.Contains("application/pdf"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UploadFileAsync(IFormFile file, Containers container, FileTypes fileType, string fileName)
        {
            string mimeType = file.ContentType;

            bool isValid = false;

            if (file == null || file.Length == 0)
            {
                return false;
            }

            long maxImageSize = 1024 * 1024 * 5;
            long maxDocumentSize = 1024 * 1024 * 20;

            if (fileType == FileTypes.Pdf || fileType == FileTypes.Excel || fileType == FileTypes.Document)
            {
                if (file.Length > maxDocumentSize)
                {
                    return false;
                }
            }
            else if (fileType == FileTypes.Image)
            {
                if (file.Length > maxImageSize)
                {
                    return false;
                }
            }

            switch (fileType)
            {
                case FileTypes.Excel:
                    if (this.IsExcel(mimeType))
                    {
                        isValid = true;
                    }
                    break;

                case FileTypes.Pdf:
                    if (this.IsPdf(mimeType))
                    {
                        isValid = true;
                    }
                    break;

                case FileTypes.Image:
                    if (this.IsImage(mimeType))
                    {
                        isValid = true;
                    }
                    break;

                case FileTypes.Document:
                    if (this.IsExcel(mimeType) || this.IsPdf(mimeType))
                    {
                        isValid = true;
                    }
                    break;
            }

            if (isValid)
            {
                string containerBlob = HelperPathAzure.MapContainerPath(container);

                using Stream stream = file.OpenReadStream();
                await this.serviceStorage.UploadBlobAsync(containerBlob, fileName, stream);

                return true;
            }

            return false;
        }
    }
}
