/*
 * Attaches CodeMirror to a text area with Id="precondition" in XML readonly mode
 */
var editor = CodeMirror.fromTextArea(document.getElementById("precondition"),
    {
        mode: "xml",
        matchClosing: true,
        smartIndent: true,
        electricChars: true,
        readonly: true
    });