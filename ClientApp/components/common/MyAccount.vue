<template>
  <b-container class="page pt-4">
    <b-row>
      <b-col cols="12">
        <div class="buttonsData">
          <b-btn
            class="buttonData"
            v-b-toggle.collapse1
            variant="outline-secondary"
          >Pokaż/Ukryj dane konta</b-btn>
        </div>
        <b-collapse id="collapse1" class="mt-2">
          <p>Dane konta:</p>
          <p>
            Adres E-mail:
            <b>{{this.$store.state.auth.email}}</b>
          </p>
          <p>
            Miasto:
            <b>{{this.$store.state.auth.city}}</b>
          </p>
          <p>
            Imię:
            <b>{{this.$store.state.auth.firstName}}</b>
          </p>
          <p>
            Nazwisko:
            <b>{{this.$store.state.auth.lastName}}</b>
          </p>
          <hr class="my-4">
        </b-collapse>
      </b-col>
    </b-row>
    <div class="middle mt-4">
      <b-col class="column col-xl-4 col-12 col-lg-5 col-md-6">
        <b-card header="<b>Zmiana adresu E-mail</b>">
          <form @submit.prevent="submitEmail('formEmail')" class="p-2">
            <b-form-group label="Nowy E-mail">
              <b-form-input
                v-model="changeEmail.email"
                @input="updateEmail"
                name="newEmail"
                data-vv-scope="formEmail"
                v-validate="'required|email'"
                autocomplete="off"
              />
              <label
                v-if="errors.first('formEmail.newEmail')"
                class="invalid-feedback"
              >{{ errors.first('formEmail.newEmail') }}</label>
            </b-form-group>
            <b-form-group label="Obecne hasło">
              <b-form-input
                type="password"
                v-model="changeEmail.password"
                @input="updateValidatePass1"
                name="validatePassword"
                data-vv-scope="formEmail"
                autocomplete="off"
                v-validate="'required'"
              />
              <label
                v-if="errors.first('formEmail.validatePassword')"
                class="invalid-feedback"
              >{{ errors.first('formEmail.validatePassword') }}</label>
            </b-form-group>
            <b-form-group class="buttons">
              <b-button variant="primary" type="submit">Zmień E-mail</b-button>
            </b-form-group>
          </form>
        </b-card>
      </b-col>
      <b-col cols="4" class="column col-xl-4 col-12 col-lg-5 col-md-6">
        <b-card header="<b>Zmiana hasła</b>">
          <form @submit.prevent="submitPass('formPass')" class="p-2">
            <b-form-group label="Obecne hasło">
              <b-form-input
                type="password"
                v-model="changePass.currentPassword"
                @input="updatePass1"
                name="currentPassword"
                data-vv-scope="formPass"
                autocomplete="off"
                v-validate="'required'"
              />
              <label
                v-if="errors.first('formPass.currentPassword')"
                class="invalid-feedback"
              >{{ errors.first('formPass.currentPassword') }}</label>
            </b-form-group>
            <b-form-group label="Nowe hasło">
              <b-form-input
                type="password"
                v-model="changePass.newPassword"
                @input="updatePass2"
                name="newPassword"
                data-vv-scope="formPass"
                autocomplete="off"
                v-validate="'required'"
              />
              <label
                v-if="errors.first('formPass.newPassword')"
                class="invalid-feedback"
              >{{ errors.first('formPass.newPassword') }}</label>
            </b-form-group>
            <b-form-group class="buttons">
              <b-button variant="primary" type="submit">Zmień hasło</b-button>
            </b-form-group>
          </form>
        </b-card>
      </b-col>
    </div>
  </b-container>
</template>

<script>
import axios from "axios";
export default {
  name: "account",
  components: {},
  data() {
    return {
      changePass: {
        currentPassword: "",
        newPassword: ""
      },
      changeEmail: {
        email: this.$store.state.auth.email,
        password: ""
      },
      changeCity: {
        city: ""
      }
    };
  },
  methods: {
    updatePass1(value) {
      this.currentPassword = value;
    },
    updatePass2(value) {
      this.newPassword = value;
    },
    updateEmail(value) {
      this.email = value;
    },
    updateValidatePass1(value) {
      this.password = value;
    },
    updateCity(value) {
      this.city = value;
    },
    submitPass(scope) {
      this.$validator.validateAll(scope).then(result => {
        if (result) {
          axios
            .put("/api/Token/updatePassword/", this.changePass)
            .then(response => {
              this.$toastr("success", "Hasło zostało zmienione.");
              const payload = {
                email: this.$store.state.auth.email,
                password: this.changePass.newPassword
              };
              this.$store
                .dispatch("updateAuth", payload)
                .then(response => {
                  this.$router.go();
                })
                .catch(error => {
                  console.log(error);
                });
            })
            .catch(error => {
              console.log(error.data);
              this.$toastr("error", "Nie udało się zmienić hasła.");
            });
        }
      });
    },
    submitEmail(scope) {
      this.$validator.validateAll(scope).then(result => {
        if (result) {
          axios
            .put("/api/Token/updateEmail/", this.changeEmail)
            .then(response => {
              const payload = {
                email: this.changeEmail.email,
                password: this.changeEmail.password
              };
              this.$store
                .dispatch("updateAuth", payload)
                .then(response => {
                  this.$router.go();
                  this.$toastr("success", "Adres E-mail został zmieniony.");
                })
                .catch(error => {
                  console.log(error);
                });
            })
            .catch(error => {
              console.log(error);
              this.$toastr("error", "Nie udało się zmienić adresu E-mail.");
            });
        }
      });
    }
  }
};
</script>
<style>
.buttons {
  text-align: center;
}
.card {
  border: none;
  background-color: #2b7a78;
}
.card-header {
  background-color: #2b7a78;
  font-size: 18px;
  text-align: center;
}
.column {
  margin: 0 auto 20px auto;
  border-radius: 10px;
  box-shadow: 3px 3px 20px rgba(0, 0, 0, 0.75);
  padding: 0;
}
.btn-outline-secondary {
  color: #feffff;
  border-color: #feffff;
}
.btn-outline-secondary:hover {
  background: #feffff;
  color: #17252a;
}
p {
  font-size: 22px;
}
@media (max-width: 420px) {
  .buttonsData {
    text-align: center;
  }

  .buttonData {
    margin-bottom: 30px;
  }
  #collapse1 p {
    font-size: 16px;
  }
}
</style>
