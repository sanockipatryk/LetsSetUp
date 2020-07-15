<template>
  <form @submit.prevent="login" class="p-2">
    <p>Zaloguj się przy pomocy adresu E-mail i hasła.</p>
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
        type="password"
        name="password"
        v-validate="'required'"
        autocomplete="off"
      />
      <label v-if="errors.first('password')" class="invalid-feedback">{{ errors.first('password') }}</label>
    </b-form-group>
    <b-form-group>
      <b-button variant="primary" type="submit" :disabled="loading">Login</b-button>
      <b-button variant="default" @click="close" :disabled="loading">Anuluj</b-button>
    </b-form-group>
  </form>
</template>
<script>
export default {
  name: "login-form",
  props: {
    registered: {
      type: Boolean,
      required: false
    }
  },
  data() {
    return {
      email: "",
      password: "",
      error: null
    };
  },
  computed: {
    loading() {
      return this.$store.state.loading;
    }
  },
  methods: {
    login() {
      this.$validator.validateAll().then(result => {
        if (result) {
          const payload = {
            email: this.email,
            password: this.password
          };
          this.$store
            .dispatch("login", payload)
            .then(response => {
              this.$toastr("success", "Zostałeś zalogowany.");
              this.error = null;
              this.email = "";
              this.password = "";
              if (this.$route.query.redirect) {
                this.$router.push(this.$route.query.redirect);
              }
            })
            .catch(error => {
              this.$toastr("error", "Logowanie nie powiodło się.");
              this.error = error.data;
            });
        }
      });
    },
    close() {
      this.$emit("close");
    }
  }
};
</script>
