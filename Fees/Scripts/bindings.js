ko.bindingHandlers.autovalidate = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        if (ko.utils.unwrapObservable(valueAccessor()) !== false) {
            // Establish a dependency on any value/checked/selectedOptions observable value
            var allBindings = allBindingsAccessor();
            var respondAfterBinding = allBindings.value || allBindings.checked || allBindings.selectedOptions;
            ko.utils.unwrapObservable(respondAfterBinding);

            // Cause jQuery Validation to revalidate this form element
            $(element).trigger("focusout");
        }
    }
};

ko.bindingHandlers.validate = {
    init: function (element, valueAccessor) {
        // Apply the validation configuration to the form
        var config = valueAccessor();
        $(element).validate().settings = $.extend(true, {}, $.validator.defaults, config);
        // Todo: replace above line with "$(element).validate(config);" after Beta
    },
    update: function (element, valueAccessor) {
        // Respond to any 'resetFormOnChange' value by removing any error messages
        ko.utils.unwrapObservable(valueAccessor().resetFormOnChange);
        $(element).validate().resetForm();
    }
};