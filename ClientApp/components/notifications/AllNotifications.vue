<template>
  <div class="wrap">
    <b-list-group v-if="notifications.length>0">
      <div class="cleanNotifications">
        <b-button variant="outline-secondary" @click.prevent="clear">Wyczyść powiadomienia</b-button>
      </div>
      <label>Nieodczytane powiadomienia</label>
      <b-list-group-item
        class="unread"
        title="Naciśnij by odczytać"
        v-for="notification in notifications"
        v-bind:key="notification.id"
        v-show="notification.isRead === false"
        @click.prevent="readN(notification, notification.eventId, notification.slug)"
      >
        <p>
          {{moment(notification.dateSend).format('DD.MM.YYYY') }}, godz.
          {{moment(notification.dateSend).format('HH:mm:ss')}}
        </p>
        <p>{{notification.content}}</p>
      </b-list-group-item>
    </b-list-group>
    <b-list-group v-if="notifications.length>0">
      <label>Odczytane powiadomienia</label>

      <b-list-group-item
        class="read"
        v-for="notification in notifications.slice(0,30)"
        v-bind:key="notification.id"
        v-show="notification.isRead === true"
        @click.prevent="onClick(notification)"
      >
        <p>
          {{moment(notification.dateSend).format('DD.MM.YYYY') }}, godz.
          {{moment(notification.dateSend).format('HH:mm:ss')}}
        </p>
        <p>{{notification.content}}</p>
      </b-list-group-item>
    </b-list-group>
    <div class="noNotifications" v-if="notifications.length===0">
      <p>Brak powiadomień</p>
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "all-notifications",
  data() {
    return {
      notifications: []
    };
  },
  methods: {
    setData(data) {
      this.notifications = data;
    },
    readN(notification, id, slug) {
      axios
        .put("/api/notifications/readNotification/" + notification.id)
        .then(response => {
          notification.isRead = true;
          this.$router.push(`/events/${id}/${slug}`);
        })
        .catch(error => {
          console.log(error.data);
        });
    },
    onClick(notification) {
      this.$router.push(`/events/${notification.eventId}/${notification.slug}`);
    },
    clear() {
      axios
        .delete("/api/notifications/clearNotifications/")
        .then(response => {
          this.$router.go();
        })
        .catch(error => {
          console.log(error.data);
        });
    }
  },
  beforeRouteEnter(to, from, next) {
    const vm = this;
    axios
      .get("/api/notifications/getAllNotifications")
      .then(response => {
        next(vm => vm.setData(response.data));
      })
      .catch(error => {
        console.log(error.data);
      });
  }
};
</script>

<style>
span {
  margin-right: 20px;
}
.list-group-item {
  background-color: #2b7a78;
  color: #feffff;
  word-wrap: break-word;
  overflow-wrap: break-word;
  font-size: 18px;
  -webkit-hyphens: auto;
  -moz-hyphens: auto;
  hyphens: auto;
}
.list-group-item.read {
  margin-bottom: 10px;
  border-radius: 20px;
  cursor: pointer;
}
.list-group-item.unread {
  cursor: pointer;
  margin-bottom: 10px;
  border-radius: 20px;
}
.list-group-item p {
  font-size: 18px;
}
.list-group.label {
  display: inline-block;
}
.cleanNotifications {
  display: inline-block;
  text-align: center;
  margin-bottom: 20px;
}
.noNotifications {
  text-align: center;
}
.noNotifications p {
  font-size: 30px;
}
</style>
