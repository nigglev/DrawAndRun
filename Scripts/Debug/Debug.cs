using UnityEditor;
using UnityEngine;


public enum ETraceLevel { Error = 0, Trace }
public static class CDebug
{
    
    public const ETraceLevel MinTraceLevel = ETraceLevel.Trace;

   public static void Trace(ETraceLevel inTL, string in_message)
    {
        if (inTL > MinTraceLevel)
            return;

        if (inTL == ETraceLevel.Error)
            Debug.LogError(in_message);
        else if (inTL == ETraceLevel.Trace)
            Debug.Log(in_message);
    }

    public static bool AssertNull(object in_obj, string in_message)
    {
        return Assert(in_obj != null, "Object is null. " + in_message);
    }
    public static bool Assert(bool in_cond, string in_message)
    {
        if (in_cond)
            return false;

        Debug.LogError(in_message);
        return true;
    }
}