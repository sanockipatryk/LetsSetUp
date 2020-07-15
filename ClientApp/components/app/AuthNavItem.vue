<template>
  <b-nav-item-dropdown v-if="isAuthenticated" right>
    <template slot="button-content">
      <i class="fas fa-user userFace"></i>
      Moje konto
    </template>
    <b-dropdown-item to="/myaccount">Zmień dane</b-dropdown-item>
    <b-dropdown-item @click="logout">
      <i class="fas fa-sign-out-alt userFace"></i>
      Wyloguj
    </b-dropdown-item>
  </b-nav-item-dropdown>
  <b-nav-item v-else @click="login">
    <i class="fas fa-user userFace"></i>
    Login / Rejestracja
  </b-nav-item>
</template>

<script>
export default {
  name: "auth-nav-item",
  computed: {
    isAuthenticated() {
      return this.$store.getters.isAuthenticated;
    },
    fullName() {
      return `${this.$store.state.auth.firstName} ${
        this.$store.state.auth.lastName
      }`;
    },
    isAdmin() {
      return this.$store.getters.isInRole("Admin");
    }
  },
  methods: {
    login() {
      this.$store.commit("showAuthModal");
    },
    logout() {
      this.$store.dispatch("logout").then(() => {
        if (this.$route.meta.requiresAuth) {
          this.$router.push("/");
        }
      });
    }
  }
};
</script>
<style>
.userFace {
  margin-right: 5px;
}
</style>
