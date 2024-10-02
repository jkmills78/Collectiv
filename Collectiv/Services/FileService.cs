using Collectiv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.Services
{
    public class FileService : IFileService
    {
        private readonly ILoggingService loggingService;

        public FileService(ILoggingService loggingService)
        {
            this.loggingService = loggingService;
        }

        public async Task<FileResult> PickAFile()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync();
                if (result != null)
                {
                    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    {
                        using var stream = await result.OpenReadAsync();
                        var image = ImageSource.FromStream(() => stream);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
                loggingService.LogException(ex);
            }

            return null;
        }
    }
}
