<template>
  <div class="app">
    <b-navbar toggleable="md">
      <b-navbar-toggle target="nav_collapse"></b-navbar-toggle>
      <b-navbar-brand to="/">Lets Set Up</b-navbar-brand>
      <b-collapse is-nav id="nav_collapse">
        <b-navbar-nav>
          <b-nav-item to="/events">Wydarzenia</b-nav-item>
          <b-nav-item v-if="isAuthenticated && isCustomer" to="/create">Utwórz wydarzenie</b-nav-item>
          <auth-nav-item/>
          <notification-dropdown v-if="isCustomer "></notification-dropdown>
        </b-navbar-nav>
      </b-collapse>
    </b-navbar>
    <transition name="fade" mode="out-in">
      <router-view/>
    </transition>
    <auth-modal :show="showAuthModal"/>
  </div>
</template>

<script>
import AuthNavItem from "./app/AuthNavItem.vue";
import AuthModal from "./app/AuthModal.vue";
import NotificationDropdown from "./notifications/NotificationDropdown.vue";
export default {
  name: "app",
  components: {
    AuthNavItem,
    AuthModal,
    NotificationDropdown
  },
  computed: {
    showAuthModal() {
      return this.$store.state.showAuthModal;
    },
    isAuthenticated() {
      return this.$store.getters.isAuthenticated;
    },
    isCustomer() {
      return (
        this.$store.getters.isInRole("Customer") ||
        !this.$store.getters.isAuthenticated
      );
    },
    isAdmin() {
      return (
        this.$store.getters.isInRole("Admin") ||
        !this.$store.getters.isAuthenticated
      );
    }
  }
};
</script>

<style>
@import url("https://fonts.googleapis.com/css?family=Roboto:400,500&subset=latin-ext");
html,
body {
  height: 100vh;
  font-family: "Roboto", sans-serif;
  background-color: #3aafa9;
  color: #def2f1;
}
div.app,
div.page {
  height: 100% !important;
  background-color: #3aafa9;
}
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease-in-out;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}
.navbar {
  background-color: #2b7a78;
}
.navbar-light .navbar-nav .nav-link.active {
  color: #feffff !important;
  font-weight: 500;
}
.navbar-light .navbar-nav .nav-link {
  color: #def2f1 !important;
}
.navbar-light .navbar-brand {
  color: #feffff !important;
  font-weight: 500;
}
.navbar-brand {
  padding: 0 50px;
  font-size: 34px;
  line-height: 50px;
  font-family: "Roboto", sans-serif;
}
.navbar-nav {
  display: block;
  width: 100%;
  font-family: "Roboto", sans-serif;
}
.nav-item {
  float: left;
  font-size: 20px;
  line-height: 100%;
}
.nav-item:nth-of-type(3) {
  float: right;
}
.nav-item:last-of-type {
  float: right;
}

@media (max-width: 1400px) and (orientation: portrait) {
  .nav-item {
    float: left;
    font-size: 16px;
  }
  .nav-item:last-of-type {
    float: right;
  }
  .navbar-brand {
    padding: 0;
    font-size: 24px;
    line-height: 50px;
  }
}
@media (max-width: 1024) and (orientation: portrait) {
  .nav-item {
    float: left;
    font-size: 16px;
  }
  .nav-item:last-of-type {
    float: right;
  }
  .navbar-brand {
    padding: 0;
    font-size: 24px;
    line-height: 50px;
  }
  .navbar-nav {
    display: block;
  }
}
@media (max-width: 768px) {
  .nav-item {
    float: left;
    font-size: 16px;
    margin-right: 0;
  }
  .nav-item:last-of-type {
    float: right;
  }
  .navbar-brand {
    padding: 0;
    font-size: 24px;
    line-height: 50px;
    font-family: "Roboto", sans-serif;
  }
  .navbar-nav {
    font-family: "Roboto", sans-serif;
  }
}

@media (max-width: 767px) and (orientation: portrait) {
  .nav-item {
    float: none;
    font-size: 16px;
  }
  .nav-item:last-of-type {
    float: none;
  }
  .navbar-brand {
    padding: 0;
    font-size: 24px;
    line-height: 50px;
  }
  .navbar-nav {
    display: block;
  }
}

@media (max-width: 400px) and (orientation: portrait) {
  .nav-item {
    float: none;
    font-size: 16px;
  }
  .nav-item:last-of-type {
    float: none;
  }
  .navbar-brand {
    padding: 0;
    font-size: 24px;
    line-height: 24px;
  }
  .navbar-nav {
    display: block;
  }
}
</style>
