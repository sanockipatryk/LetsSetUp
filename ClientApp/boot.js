import Vue from 'vue';
import VueRouter from 'vue-router';
import Catalogue from "./pages/Catalogue.vue";
import Event from "./pages/Event.vue";
import BootstrapVue from "bootstrap-vue";
import NProgress from "nprogress";
import moment from 'moment';
import store from "./store";
import axios from "axios";
import VeeValidate from 'vee-validate';
import "./helpers/validation";
import VueToastr from "@deveodk/vue-toastr";
import "@deveodk/vue-toastr/dist/@deveodk/vue-toastr.css";
import CreateEvent from "./components/events/CreateEvent.vue";
import EditEvent from "./components/events/EditEvent.vue";
import AllNotifications from "./components/notifications/AllNotifications.vue"
import MyAccount from "./components/common/MyAccount.vue";


Vue.use(VueRouter);
Vue.use(BootstrapVue);
Vue.use(VeeValidate);
Vue.use(VueToastr, {
    defaultPosition: "toast-top-right"
});

Vue.prototype.moment = moment;
moment.locale('pl');

const initialStore = localStorage.getItem("store");
if (initialStore) {
    store.commit("initialise", JSON.parse(initialStore));
    if (store.getters.isAuthenticated) {
        axios.defaults.headers.common["Authorization"] = `Bearer ${
      store.state.auth.access_token
    }`;
    }
}

const routes = [{
        path: "/events",
        component: Catalogue,
        meta: {
            title: "Wydarzenia - LetsSetUp"
        }
    },
    {
        path: "/events/:id/:slug",
        component: Event,
        meta: {
            title: "Szczegóły - LetsSetUp"
        }
    },
    {
        path: "/notifications",
        component: AllNotifications,
        meta: {
            requiresAuth: true,
            role: "Customer",
            title: "Powiadomienia - LetsSetUp"
        }
    },
    {
        path: "/create",
        component: CreateEvent,
        meta: {
            requiresAuth: true,
            role: "Customer",
            title: "Dodaj wydarzenie - LetsSetUp"
        }
    },
    {
        path: "/events/:id/:slug/edit",
        component: EditEvent,
        meta: {
            requiresAuth: true,
            role: "Customer",
            title: "Edytuj wydarzenie - LetsSetUp"
        }
    },
    {
        path: "/myaccount",
        component: MyAccount,
        meta: {
            requiresAuth: true,
            title: "Moje konto - LetsSetUp"
        }
    },
    {
        path: "*",
        redirect: "/events"
    }
];
const router = new VueRouter({
    mode: "history",
    routes: routes
});
router.beforeEach((to, from, next) => {
    NProgress.start();
    document.title = to.meta.title
    if (to.matched.some(route => route.meta.requiresAuth)) {
        if (!store.getters.isAuthenticated) {
            store.commit("showAuthModal");
            next({
                path: from.path,
                query: {
                    redirect: to.path
                }
            });
        } else {
            if (
                to.matched.some(
                    route => route.meta.role && store.getters.isInRole(route.meta.role)
                )
            ) {
                next();
            } else if (!to.matched.some(route => route.meta.role)) {
                next();
            } else {
                next({
                    path: "/"
                });
            }
        }
    } else {
        if (
            to.matched.some(
                route =>
                route.meta.role &&
                (!store.getters.isAuthenticated ||
                    store.getters.isInRole(route.meta.role))
            )
        ) {
            next();
        } else {
            if (to.matched.some(route => route.meta.role)) {
                next({
                    path: "/"
                });
            }

            next();
        }
    }
});
router.afterEach((to, from) => {
    NProgress.done();
});
new Vue({
    el: "#app-root",
    router: router,
    store,
    render: h => h(require("./components/App.vue"))
});