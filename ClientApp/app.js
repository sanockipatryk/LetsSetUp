import Vue from "vue";
import VeeValidate from "vee-validate";
import router from "./router";
import store from "./store";
import {
    sync
} from "vuex-router-sync";
import Cookie from "js-cookie";
import axios from "axios";
import "./helpers/interceptors";
import "./helpers/validation";
import App from "./components/App.vue";

Vue.use(VeeValidate);

const auth = Cookie.get("AUTH");
if (auth) {
    store.commit("loginSuccess", JSON.parse(auth));
    axios.defaults.headers.common["Authorization"] = `Bearer ${
    store.state.auth.access_token
  }`;
}

const app = new Vue({
    router,
    store,
    render: h => h(App)
});

export {
    app,
    router,
    store
};