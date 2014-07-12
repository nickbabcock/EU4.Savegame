using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Stats.Web
{
    /// <summary>
    /// Class that will be serialized to the client on error.
    /// It contains a user friendly and developer friendly
    /// messages.
    /// </summary>
    public class ErrorMessage
    {
        public ErrorMessage(Exception e)
        {
            this.Error = new InnerError();
            this.Error.Message = e.Message;
            this.Error.Developer = e.Source + Environment.NewLine + e.StackTrace;
        }

        /// <summary>
        /// Gets or sets the wrapped messages in an overall "error" object
        /// </summary>
        public InnerError Error { get; set; }

        public class InnerError
        {
            /// <summary>
            /// Gets or sets the user friendly message that is to 
            /// be displayed in the interface to let the client
            /// know that something is wrong
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Gets or sets the developer friendly message that should
            /// aid in debugging given that there is a problem with the
            /// service
            /// </summary>
            public string Developer { get; set; }
        }
    }
}
