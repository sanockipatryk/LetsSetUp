<template>
  <b-container class="notifi">
    <b-nav-item-dropdown
      v-bind:text="`Powiadomienia: ` + notifications.length "
      v-if="isAuthenticated"
      right
    >
      <b-dropdown-item
        v-for="notification in notifications"
        v-show="notification.isRead === false"
        v-bind:key="notification.id"
        v-bind:title="notification.content"
        @click.prevent="read(notification, notification.eventId, notification.slug)"
      >{{shorten(notification.content)}}</b-dropdown-item>
      <b-dropdown-header v-if="notifications.length === 0">Brak nowych powiadomień</b-dropdown-header>
      <b-dropdown-divider></b-dropdown-divider>
      <b-button
        style="margin: 0 auto; display: block"
        variant="outline-primary"
        @click.prevent="viewMore"
      >Zobacz wszystkie</b-button>
    </b-nav-item-dropdown>
  </b-container>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      notifications: []
    };
  },
  computed: {
    isAuthenticated() {
      return this.$store.getters.isAuthenticated;
    }
  },
  methods: {
    setData(response) {
      this.notifications = response;
    },
    shorten(text) {
      if (text !== undefined) {
        if (text.length > 100) {
          return text.substring(0, 97) + "...";
        } else return text;
      }
    },
    read(notification, id, slug) {
      axios
        .put("/api/notifications/readNotification/" + notification.id)
        .then(response => {
          this.deleteNotification(notification);
          this.$router.push(`/events/${id}/${slug}`);
        })
        .catch(error => {
          console.log(error.data);
        });
    },
    deleteNotification: function(notification) {
      this.notifications.splice(this.notification);
    },
    viewMore() {
      this.$router.push(`/notifications`);
    }
  },
  watch: {
    $route: _.debounce(function(to, from) {
      const vm = this;
      if (this.$store.getters.isAuthenticated) {
        axios
          .get("/api/notifications/getNotifications")
          .then(response => this.setData(response.data))
          .catch(error => {
            console.log(error.data);
          });
      }
    }, 500),
    notifications(value) {
      this.notifications = value;
    }
  },
  created() {
    const vm = this;
    if (this.$store.getters.isAuthenticated) {
      axios
        .get("/api/notifications/getNotifications")
        .then(response => this.setData(response.data))
        .catch(error => {
          console.log(error.data);
        });
    }
  }
};
</script>

<style>
.dropdown-menu {
  min-width: 200px;
}
.notifi {
  margin-left: 300px;
  margin-right: 0;
}
@media (max-width: 992px) {
  .dropdown-menu {
    max-width: 350px;
  }
  a.dropdown-item {
    max-width: 320px;
    overflow: scroll;
  }
  .notifi {
    margin-left: 0;
    margin-right: 0;
  }
}
</style>
