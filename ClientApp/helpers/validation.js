import axios from "axios";
import VeeValidate, {
    Validator
} from "vee-validate";

const dictionary = {
    en: {
        messages: {
            required: () => 'To pole jest wymagane',
            min: () => 'Zbyt krótka zawartość',
            url: () => 'Zawartość musi być adresem internetowym',
            email: () => 'Podaj poprawny adres e-mail',
            confirmed: () => 'Hasła nie zgadzają się',
            min_value: () => 'Minimalna wartość to 1',
            numeric: () => 'To pole przyjmuje tylko liczby'
        }
    }
};

// Override and merge the dictionaries
Validator.localize(dictionary);

const isLaterThan = (value) => {
    var date = new Date();
    var newVal = new Date(value);
    return newVal >= date;
};

Validator.extend("minDate", {
    validate: isLaterThan,
    getMessage: (field, params, data) => "Ta data musi mieć miejsce w przyszłości"
});