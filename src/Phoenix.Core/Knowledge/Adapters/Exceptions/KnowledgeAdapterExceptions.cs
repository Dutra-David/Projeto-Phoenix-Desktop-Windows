using System;
using System.Runtime.Serialization;

namespace Phoenix.Core.Knowledge.Adapters.Exceptions
{
    /// <summary>
    /// Base exception for all knowledge adapter-related errors.
    /// </summary>
    [Serializable]
    public class KnowledgeAdapterException : Exception
    {
        /// <summary>
        /// Gets the error code for categorizing the exception.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets the operation that was being performed when the error occurred.
        /// </summary>
        public string FailedOperation { get; set; }

        public KnowledgeAdapterException(string message, string errorCode = "UNKNOWN_ERROR", string failedOperation = null)
            : base(message)
        {
            ErrorCode = errorCode;
            FailedOperation = failedOperation;
        }

        public KnowledgeAdapterException(string message, Exception innerException, string errorCode = "UNKNOWN_ERROR", string failedOperation = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            FailedOperation = failedOperation;
        }

        protected KnowledgeAdapterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ErrorCode = info.GetString(nameof(ErrorCode));
            FailedOperation = info.GetString(nameof(FailedOperation));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(ErrorCode), ErrorCode);
            info.AddValue(nameof(FailedOperation), FailedOperation);
        }
    }

    /// <summary>
    /// Thrown when a knowledge item is not found in the store.
    /// </summary>
    [Serializable]
    public class KnowledgeItemNotFoundException : KnowledgeAdapterException
    {
        public string ItemId { get; set; }

        public KnowledgeItemNotFoundException(string itemId, string message = null)
            : base(message ?? $"Knowledge item with ID '{itemId}' was not found.", "ITEM_NOT_FOUND", "GetById")
        {
            ItemId = itemId;
        }

        public KnowledgeItemNotFoundException(string itemId, Exception innerException)
            : base($"Knowledge item with ID '{itemId}' was not found.", innerException, "ITEM_NOT_FOUND", "GetById")
        {
            ItemId = itemId;
        }

        protected KnowledgeItemNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ItemId = info.GetString(nameof(ItemId));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(ItemId), ItemId);
        }
    }

    /// <summary>
    /// Thrown when an adapter fails to initialize.
    /// </summary>
    [Serializable]
    public class AdapterInitializationException : KnowledgeAdapterException
    {
        public string AdapterType { get; set; }

        public AdapterInitializationException(string adapterType, string message, Exception innerException = null)
            : base(message, innerException, "INIT_FAILED", "Initialize")
        {
            AdapterType = adapterType;
        }

        protected AdapterInitializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            AdapterType = info.GetString(nameof(AdapterType));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(AdapterType), AdapterType);
        }
    }

    /// <summary>
    /// Thrown when input validation fails.
    /// </summary>
    [Serializable]
    public class ValidationException : KnowledgeAdapterException
    {
        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }

        public ValidationException(string propertyName, object propertyValue, string message)
            : base(message, "VALIDATION_FAILED", "Validate")
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            PropertyName = info.GetString(nameof(PropertyName));
            PropertyValue = info.GetValue(nameof(PropertyValue), typeof(object));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(PropertyName), PropertyName);
            info.AddValue(nameof(PropertyValue), PropertyValue);
        }
    }

    /// <summary>
    /// Thrown when a concurrency conflict is detected (optimistic locking failure).
    /// </summary>
    [Serializable]
    public class ConcurrencyException : KnowledgeAdapterException
    {
        public int ExpectedVersion { get; set; }
        public int ActualVersion { get; set; }

        public ConcurrencyException(int expectedVersion, int actualVersion)
            : base(
                $"Concurrency conflict detected. Expected version {expectedVersion} but found {actualVersion}.",
                "CONCURRENCY_CONFLICT",
                "Update")
        {
            ExpectedVersion = expectedVersion;
            ActualVersion = actualVersion;
        }

        protected ConcurrencyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ExpectedVersion = info.GetInt32(nameof(ExpectedVersion));
            ActualVersion = info.GetInt32(nameof(ActualVersion));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(ExpectedVersion), ExpectedVersion);
            info.AddValue(nameof(ActualVersion), ActualVersion);
        }
    }

    /// <summary>
    /// Thrown when a data access operation fails.
    /// </summary>
    [Serializable]
    public class DataAccessException : KnowledgeAdapterException
    {
        public string DatabaseName { get; set; }
        public string QueryType { get; set; }

        public DataAccessException(string message, string databaseName = null, string queryType = null, Exception innerException = null)
            : base(message, innerException, "DATA_ACCESS_ERROR", "ExecuteQuery")
        {
            DatabaseName = databaseName;
            QueryType = queryType;
        }

        protected DataAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            DatabaseName = info.GetString(nameof(DatabaseName));
            QueryType = info.GetString(nameof(QueryType));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(DatabaseName), DatabaseName);
            info.AddValue(nameof(QueryType), QueryType);
        }
    }

    /// <summary>
    /// Thrown when the adapter is unable to process due to rate limiting or throttling.
    /// </summary>
    [Serializable]
    public class RateLimitExceededException : KnowledgeAdapterException
    {
        public int RequestCount { get; set; }
        public int MaxAllowedRequests { get; set; }
        public TimeSpan TimeWindow { get; set; }

        public RateLimitExceededException(int requestCount, int maxAllowed, TimeSpan timeWindow)
            : base(
                $"Rate limit exceeded. {requestCount} requests in {timeWindow.TotalSeconds}s. Maximum allowed: {maxAllowed}",
                "RATE_LIMIT_EXCEEDED",
                "Execute")
        {
            RequestCount = requestCount;
            MaxAllowedRequests = maxAllowed;
            TimeWindow = timeWindow;
        }

        protected RateLimitExceededException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            RequestCount = info.GetInt32(nameof(RequestCount));
            MaxAllowedRequests = info.GetInt32(nameof(MaxAllowedRequests));
            TimeWindow = (TimeSpan)info.GetValue(nameof(TimeWindow), typeof(TimeSpan));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(RequestCount), RequestCount);
            info.AddValue(nameof(MaxAllowedRequests), MaxAllowedRequests);
            info.AddValue(nameof(TimeWindow), TimeWindow);
        }
    }

    /// <summary>
    /// Thrown when a required resource (index, table, connection) is not found.
    /// </summary>
    [Serializable]
    public class ResourceNotFoundException : KnowledgeAdapterException
    {
        public string ResourceType { get; set; }
        public string ResourceName { get; set; }

        public ResourceNotFoundException(string resourceType, string resourceName)
            : base(
                $"{resourceType} '{resourceName}' was not found.",
                "RESOURCE_NOT_FOUND",
                "FindResource")
        {
            ResourceType = resourceType;
            ResourceName = resourceName;
        }

        protected ResourceNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ResourceType = info.GetString(nameof(ResourceType));
            ResourceName = info.GetString(nameof(ResourceName));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(ResourceType), ResourceType);
            info.AddValue(nameof(ResourceName), ResourceName);
        }
    }
}
