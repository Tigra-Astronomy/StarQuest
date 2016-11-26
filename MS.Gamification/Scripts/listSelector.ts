var checkboxes = $(".data:checkbox");
$(document)
    .ready(() =>
        {
        $(":checkbox#chkSelectAll")
            .change(function()
                {
                checkboxes.prop("checked", $(this).prop("checked"));
                });
        $("#btn-select-all")
            .click(() => { selectUsers(selectAll) });
        $("#btn-select-none")
            .click(() => { selectUsers(selectNone) });
        $("#btn-invalid-password")
            .click(() => { selectUsers(invalidPassword); });
        $("#btn-unverified-email")
            .click(() => { selectUsers(unverifiedEmail); });
        $("#btn-locked-out")
            .click(() => { selectUsers(locked); });
        $("#btn-invert-selection")
            .click(() => { selectUsers(invertSelection); });
        $("#users").DataTable();
        });

interface IInputElementPredicate
    {
    (element: HTMLInputElement): boolean;
    }

function selectUsers(predicate: IInputElementPredicate)
    {
    checkboxes.prop("checked",
        (index, oldValue) =>
            {
            return predicate(checkboxes[index] as HTMLInputElement);
            });
    }

function unverifiedEmail(element: HTMLInputElement)
    {
    return element.attributes["data-email-verified"].value === "False";
    }

function invalidPassword(element: HTMLInputElement)
    {
    return element.attributes["data-has-password"].value === "False";
    }

function locked(element: HTMLInputElement) { return element.attributes["data-account-locked"].value === "True"; }

function selectNone(element: HTMLInputElement) { return false; }

function selectAll(element: HTMLInputElement) { return true; }

function invertSelection(element: HTMLInputElement)
    {
    return !element.checked;
    }