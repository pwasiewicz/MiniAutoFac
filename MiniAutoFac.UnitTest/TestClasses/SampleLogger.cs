namespace MiniAutoFac.UnitTest.TestClasses
{
    using System;

    class SampleLogger
    {
        public Type TypeToLog { get; set; }

        public SampleLogger(Type typeToLog)
        {
            this.TypeToLog = typeToLog;
        }
    }

    class ClassThatLogs
    {
        private readonly SampleLogger logger;
        public ClassThatLogs(SampleLogger logger)
        {
            this.logger = logger;
        }

        public Type GetLoggedType
        {
            get { return this.logger.TypeToLog; }
        }
    }
}
