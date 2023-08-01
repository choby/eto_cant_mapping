// using Sentry;
// using Volo.Abp.ExceptionHandling;
//
// namespace Evo.Scm.ExceptionHandling;
//
// public class SentryExceptionSubscriber : ExceptionSubscriber
// {
//     public override Task HandleAsync(ExceptionNotificationContext context)
//     {
//         if(!context.Handled)
//             SentrySdk.CaptureException(context.Exception);
//         return Task.CompletedTask;
//     }
// }