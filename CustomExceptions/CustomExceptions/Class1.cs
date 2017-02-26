using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomExceptions
{
    /*
     * All the custom exceptions to be used/created in this project
     * should be defined here
     * 
     * @Author: shibasis sengupta
    */

    public class invalidParamException:System.Exception
    {
        private String desc="Incomplete or Invalid Parameter Passed";
       
        public invalidParamException(String src)
        {
            desc = desc + " source: " + src;
        }
        
        public String toString()
        {
            return desc;
        }


     }

    public class businessRuleViolationException : System.Exception
    {
        private String desc = "In conflict with business rules";

        public businessRuleViolationException(String src)
        {
            desc = desc + " source: " + src;
        }
        public String toString()
        {
            return desc;
        }
    }
}
