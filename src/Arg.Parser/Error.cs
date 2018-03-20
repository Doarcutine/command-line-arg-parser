namespace Arg.Parser
{
    /// <summary>
    /// Parse error dto
    /// </summary>
    public class Error
    {
        internal Error(ParsingErrorCode code, string errorDetail, string trigger)
        {
            this.Code = code;
            this.Detail = errorDetail;
            this.Trigger = trigger;
        }

        /// <summary>
        /// input flag which trigger error
        /// </summary>
        public string Trigger { get; }

        /// <summary>
        /// error detail
        /// </summary>
        public string Detail { get; }

        /// <summary>
        /// error code
        /// </summary>
        public ParsingErrorCode Code { get; }
    }

    /// <summary>
    /// ParsingErrorCode
    /// </summary>
    public enum ParsingErrorCode
    {
        /// <summary>
        /// input argument is not supported or can't be parse
        /// </summary>
        FreeValueNotSupported,
        /// <summary>
        /// input argument duplicate(ignore case) 
        /// </summary>
        DuplicateFlagsInArgs
    }
}