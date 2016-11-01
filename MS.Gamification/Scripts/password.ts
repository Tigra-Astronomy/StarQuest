$(document)
    .ready(() => {
        $("input[type=password]")
            .keyup(validatePassword)
            .focus(() => { $("#passwordInfo").show(); })
            .blur(() => { $("#passwordInfo").hide(); });
    });

function setValidationState(id: string, isValid: boolean) {
    const cssId = `#${id}`;
    const icon = `#${id}>i`;

    if (isValid) {
        $(cssId).removeClass("pw-invalid").addClass("pw-valid");
        $(icon).removeClass("fa-warning").addClass("fa-check");
    } else {
        $(cssId).removeClass("pw-valid").addClass("pw-invalid");
        $(icon).removeClass("fa-check").addClass("fa-warning");
    }
}

function isMatch(candidate: string, pattern: RegExp): boolean {
    const matches = candidate.match(pattern);
    return !(matches == null);
}

function validatePassword() {
    const password: string = $(this).val();
    const containsLowercase = isMatch(password, /[a-z]/);
    const containsUppercase = isMatch(password, /[A-Z]/);
    const containsDigits = isMatch(password, /[0-9]/);
    const containsPunctuation = isMatch(password, /[^\w\d]/);
    const userName: string = $("#UserName").val();
    const doesNotContainUserName = password.indexOf(userName) < 0;
    const complexityTests = [containsLowercase, containsUppercase, containsDigits, containsPunctuation];
    const passedTests = countTrue(complexityTests);
    const isComplex = passedTests >= 3;
    const isOfSufficientLength = password.length >= 8;
    const submittableTests = [isComplex, isOfSufficientLength, doesNotContainUserName];
    setValidationState("pw-length", isOfSufficientLength);
    setValidationState("pw-lowercase", containsLowercase);
    setValidationState("pw-uppercase", containsUppercase);
    setValidationState("pw-digit", containsDigits);
    setValidationState("pw-punctuation", containsPunctuation);
    setValidationState("pw-username", doesNotContainUserName);
    setValidationState("pw-complexity", isComplex);
    setSubmitButtonState(submittableTests);
}

function countTrue(items: boolean[]): number {
    var total = 0;
    items.forEach((x) => {
        if (x) {
            ++total;
        }
    });
    return total;
}

function setSubmitButtonState(conditions: boolean[]) {
    const button = $("input[type=submit]");
    const isValid = countTrue(conditions) === conditions.length;
    if (isValid) {
        button.removeAttr("disabled");
    } else {
        button.attr("disabled", "disabled");
    }
}