/*
 * Attaches CodeMirror to a text area with Id="precondition" in XML editing mode
 */
var editor = CodeMirror.fromTextArea(document.getElementById("precondition"),
    {
        mode: "xml",
        matchClosing: true,
        smartIndent: true,
        electricChars: true,
        readonly: false
    });