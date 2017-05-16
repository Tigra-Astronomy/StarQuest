var checkboxes = $(".data:checkbox");
$(document)
    .ready(function () {
    $(":checkbox#chkSelectAll")
        .change(function () {
        checkboxes.prop("checked", $(this).prop("checked"));
    });
    $("#btn-select-all")
        .click(function () { selectUsers(selectAll); });
    $("#btn-select-none")
        .click(function () { selectUsers(selectNone); });
    $("#btn-invalid-password")
        .click(function () { selectUsers(invalidPassword); });
    $("#btn-unverified-email")
        .click(function () { selectUsers(unverifiedEmail); });
    $("#btn-locked-out")
        .click(function () { selectUsers(locked); });
    $("#btn-invert-selection")
        .click(function () { selectUsers(invertSelection); });
    $("#users").DataTable();
});
function selectUsers(predicate) {
    checkboxes.prop("checked", function (index, oldValue) {
        return predicate(checkboxes[index]);
    });
}
function unverifiedEmail(element) {
    return element.attributes["data-email-verified"].value === "False";
}
function invalidPassword(element) {
    return element.attributes["data-has-password"].value === "False";
}
function locked(element) { return element.attributes["data-account-locked"].value === "True"; }
function selectNone(element) { return false; }
function selectAll(element) { return true; }
function invertSelection(element) {
    return !element.checked;
}
//# sourceMappingURL=listSelector.js.map