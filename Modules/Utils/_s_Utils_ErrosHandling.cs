using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Utils
{
    public class _s_Utils_ErrosHandling
    {
        /// <summary>
        /// The sueDB is object of Error handling database publicly on this class
        /// use at _s_Utils_ErrosHandling for init
        /// use at AddErrorToErrorList for add a Error to DB
        /// use at RemoveErrorFromDB for Removes all erros before date from DB
        /// </summary>
        _s_Utils_ErrosHandling_DB sueDB = null;
        /// <summary>
        /// The error list
        /// </summary>
        private List<ErrorDescriptor> ErrorList;
        /// <summary>
        /// the class is node of every error
        /// </summary>
        public class ErrorDescriptor
        {
            public string MethodName;
            public string ExeptionMessage;
            public DateTime DateOfError;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="_s_Utils_ErrosHandling"/> class.
        /// </summary>
        public _s_Utils_ErrosHandling()
        {
            ErrorList = new List<ErrorDescriptor>();
            sueDB = new _s_Utils_ErrosHandling_DB();
        }

        /// <summary>
        /// Execute each of the specified action, and if the action is failed, go and executes the next action.
        /// </summary>
        /// <param name="actions">The actions.</param>
        public void OnErrorResumeNext(params Action[] actions)
        {
            OnErrorResumeNext(actions: actions, returnExceptions: false);
        }

        /// <summary>
        /// Execute each of the specified action, and if the action is failed go and executes the next action.
        /// </summary>
        /// <param name="returnExceptions">if set to <c>true</c> return list of exceptions that were thrown by the actions that were executed.</param>
        /// <param name="putNullWhenNoExceptionIsThrown">if set to <c>true</c> and <paramref name="returnExceptions"/> is also <c>true</c>, put <c>null</c> value in the returned list of exceptions for each action that did not threw an exception.</param>
        /// <param name="actions">The actions.</param>
        /// <returns>List of exceptions that were thrown when executing the actions.</returns>
        /// <remarks>
        /// If you set <paramref name="returnExceptions"/> to <c>true</c>, it is possible to get exception thrown when trying to add exception to the list. 
        /// Note that this exception is not handled!
        /// </remarks>
        public Exception[] OnErrorResumeNext(bool returnExceptions = false, bool putNullWhenNoExceptionIsThrown = false, params Action[] actions)
        {
            var exceptions = returnExceptions ? new System.Collections.Generic.List<Exception>() : null;
            foreach (var action in actions)
            {
                Exception exp = null;
                try { action.Invoke(); }
                catch (Exception ex) {
                    AddErrorToErrorList(action.Method.Name, ex.Message);
                    if (returnExceptions) {
                        exp = ex;
                    } }

                if (exp != null || putNullWhenNoExceptionIsThrown) { exceptions.Add(exp); }
            }
            return exceptions?.ToArray();
        }

        /// <summary>
        /// Adds the error to error list.
        /// </summary>
        /// <param name="MethodName">Name of the method.</param>
        /// <param name="ExeptionMessage">The exeption message.</param>
        public void AddErrorToErrorList(string MethodName, string ExeptionMessage)
        {
            ErrorDescriptor errordescriptor = new ErrorDescriptor();
            errordescriptor.MethodName = MethodName;
            errordescriptor.ExeptionMessage = ExeptionMessage;
            errordescriptor.DateOfError = DateTime.Now;
            //lock(ErrorList)
            //{
                ErrorList.Add(errordescriptor);
            //}
            sueDB.AddErrorToDB(errordescriptor);
                       
        }
        /// <summary>
        /// Removes the error from database.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        public void RemoveErrorFromDB(DateTime datetime)
        {
            sueDB.RemoveAllErrosBeforeDateFromDB(datetime);
        }
        /// <summary>
        /// Gets the error list.
        /// </summary>
        /// <returns>error descriptor list from current function</returns>
        public List<ErrorDescriptor> GetAliveErrorList()
        {
            return ErrorList;
        }
        /// <summary>
        /// Gets the database error list.
        /// </summary>
        /// <returns>error descriptor list from db for all error recorded on it</returns>
        public List<ErrorDescriptor> GetDbErrorList()
        {
            return sueDB.GetAllErrors();
        }
    }
}
