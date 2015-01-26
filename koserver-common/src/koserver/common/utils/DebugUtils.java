package koserver.common.utils;

public class DebugUtils {

    static boolean isDebugEnabled = true;
    static boolean printConsole = true;
    static boolean printFile = true;

    public static void Trace(String data) {
        if (isDebugEnabled) {
            if (printConsole) {
                System.out.println(data);
            }
            // TODO: Print to file if printFile is true
        }
    }
}
