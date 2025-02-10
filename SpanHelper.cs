using System;
using Instana.ManagedTracing.Api;
using Instana.ManagedTracing.Sdk.Spans;

namespace Instana.SpanHelper
{
    public static class SpanHelper
    {
        private const string Error = "error";
        private const string Warning = "warning";
        private const string Success = "success";
        private const string Message = "message";
        private const string Stack = "stack";
        private const string Ex = "ex";

        public static CustomEntrySpan StartEntryForNewTrace(object instrumentedObject, string operationName)
        {
         
            return CustomSpan.CreateEntryForNewTrace(instrumentedObject, operationName);
        }

        public static CustomEntrySpan StartEntry(object instrumentedObject, string operationName)
        {
            return CustomSpan.CreateEntry(instrumentedObject, (ISpanContext)null, operationName);
        }

        public static CustomSpan StartIntermediate(object instrumentedObject, string operationName)
        {
            return CustomSpan.Create(instrumentedObject, SpanType.INTERMEDIATE, operationName);
        }

        public static CustomExitSpan StartExit(object instrumentedObject, string operationName)
        {
            return CustomSpan.CreateExit(instrumentedObject, null, operationName);
        }

        public static void SetError(this CustomSpan span, Exception ex = null, string message = null)
        {
            if (span == null) return;

            span.SetTag(Error, "true");
            if (!string.IsNullOrEmpty(message))
            {
                span.SetTag(new[] { Error, Message }, message);
            }


            if (ex != null)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    span.SetTag(new[] { Error, Ex, Message }, ex.Message);
                }

                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    span.SetTag(new[] { Error, Stack }, ex.StackTrace);
                }
            }
        }

        public static void SetWarning(this CustomSpan span, string value)
        {
            if (span == null) return;

            span.SetTag(Warning, "true");
            span.SetTag(new[] { Warning, Message }, value);
        }

        public static void SetSuccess(this CustomSpan span, string message = "Success")
        {
            if (span == null) return;

            span.SetTag(Success, "true");
            span.SetTag(new[] { Success, Message }, message);
        }

        public static void SetLog(this CustomSpan span, string key, string value)
        {
            span?.SetTag(key, value);
        }

        public static void WrapAction(this CustomSpan span, Action action, bool rethrowExceptions = true)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                span.SetError(ex);
                if (!rethrowExceptions)
                    return;
                throw;
            }
        }

        public static T Wrap<T>(this CustomSpan span, Func<T> action, bool rethrowExceptions = true)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                span.SetError(ex);
                if (!rethrowExceptions)
                    return default(T);
                throw;
            }
        }
    }
}
