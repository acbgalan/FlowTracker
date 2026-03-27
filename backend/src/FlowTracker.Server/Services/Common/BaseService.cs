using FlowTracker.Shared.Dtos.Common;
using Microsoft.EntityFrameworkCore;

namespace FlowTracker.Server.Services.Common
{
    public abstract class BaseService
    {
        public ServiceResult SuccessResult(string message, int statusCode)
        {
            return new ServiceResult
            {
                Success = true,
                Message = message,
                StatusCode = statusCode
            };
        }

        public ServiceResult<T> SuccessResult<T>(string message, int statusCode, T? data = default)
        {
            return new ServiceResult<T>
            {
                Success = true,
                Message = message,
                StatusCode = statusCode,
                Data = data
            };
        }

        public ServiceResult FailureResult(string message, int statusCode)
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                StatusCode = statusCode
            };
        }

        public ServiceResult<T> FailureResult<T>(string message, int statusCode, T? data = default)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
                Data = data
            };
        }

        public ServiceResult HandleDbUpdateException(DbUpdateException ex)
        {
            return FailureResult($"Database error: {ex.Message}", StatusCodes.Status500InternalServerError);
        }

        public ServiceResult<T> HandleDbUpdateException<T>(DbUpdateException ex)
        {
            return FailureResult<T>($"Database error: {ex.Message}", StatusCodes.Status500InternalServerError);
        }

        public ServiceResult HandleGeneralException(Exception ex)
        {
            return FailureResult($"Unexpected error: {ex.Message}", StatusCodes.Status500InternalServerError);
        }

        public ServiceResult<T> HandleGeneralException<T>(Exception ex)
        {
            return FailureResult<T>($"Unexpected error: {ex.Message}", StatusCodes.Status500InternalServerError);
        }

    }
}
