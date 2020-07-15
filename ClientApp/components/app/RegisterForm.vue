<template>
  <form @submit.prevent="submit" class="p-2" autocomplete="off">
    <b-form-group label="Imię">
      <b-form-input
        v-model.trim="firstName"
        name="firstName"
        v-validate="'required'"
        autocomplete="off"
      />
      <label
        v-if="errors.first('firstName')"
        class="invalid-feedback"
      >{{ errors.first('firstName') }}</label>
    </b-form-group>
    <b-form-group label="Nazwisko">
      <b-form-input
        v-model.trim="lastName"
        name="lastName"
        v-validate="'required'"
        autocomplete="off"
      />
      <label v-if="errors.first('lastName')" class="invalid-feedback">{{ errors.first('lastName') }}</label>
    </b-form-group>
    <b-form-group label="Miasto">
      <b-form-input v-model.trim="city" name="city" v-validate="'required'" autocomplete="off"/>
      <label v-if="errors.first('city')" class="invalid-feedback">{{ errors.first('city') }}</label>
    </b-form-group>
    <b-form-group label="E-mail">
      <b-form-input
        v-model.trim="email"
        name="email"
        v-validate="'required|email'"
        autocomplete="off"
      />
      <label v-if="errors.first('email')" class="invalid-feedback">{{ errors.first('email') }}</label>
    </b-form-group>
    <b-form-group label="Hasło">
      <b-form-input
        v-model.trim="password"
        name="password"
        type="password"
        v-validate="'required'"
        autocomplete="off"
      />
      <label v-if="errors.first('password')" class="invalid-feedback">{{ errors.first('password') }}</label>
    </b-form-group>
    <b-form-group label="Potwierdź hasło">
      <b-form-input
        v-model.trim="confirmPassword"
        name="confirmPassword"
        type="password"
        v-validate="'required'"
        autocomplete="off"
      />
      <label
        v-if="errors.first('confirmPassword')"
        class="invalid-feedback"
      >{{ errors.first('confirmPassword') }}</label>
    </b-form-group>
    <b-form-group>
      <b-button variant="primary" type="submit" :disabled="loading">Zarejestruj</b-button>
      <b-button variant="default" @click="close" :disabled="loading">Anuluj</b-button>
    </b-form-group>
  </form>
</template>

<script>
export default {
  name: "register-form",
  data() {
    return {
      firstName: "",
      lastName: "",
      city: "",
      email: "",
      password: "",
      confirmPassword: "",
      regErrors: null
    };
  },
  computed: {
    loading() {
      return this.$store.state.loading;
    }
  },
  methods: {
    submit() {
      this.$validator.validateAll().then(result => {
        if (result) {
          const payload = {
            firstName: this.firstName,
            lastName: this.lastName,
            city: this.city,
            email: this.email,
            password: this.password,
            confirmPassword: this.confirmPassword
          };
          this.$store
            .dispatch("register", payload)
            .then(response => {
              this.regErrors = null;
              this.firstName = "";
              this.lastName = "";
              this.city = "";
              this.email = "";
              this.password = "";
              this.confirmPassword = "";
              this.$toastr("success", "Zostałeś zarejestrowany.");
              this.$emit("success");
            })
            .catch(error => {
              this.$toastr("error", "Rejestracja nie powiodła się.");

              if (
                typeof error.data === "string" ||
                error.data instanceof String
              ) {
                this.regErrors = { error: [error.data] };
              } else {
                this.regErrors = error.data;
              }
            });
        }
      });
    },
    close() {
      this.regErrors = null;
      this.$emit("close");
    }
  }
};
</script>
