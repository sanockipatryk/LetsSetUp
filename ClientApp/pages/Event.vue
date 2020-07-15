<template>
  <div class="page">
    <event-details v-if="event" :signed="signed" :event="event" :comms="comments"/>
  </div>
</template>
<script>
import EventDetails from "../components/events/Details.vue";
import NProgress from "nprogress";
import axios from "axios";

export default {
  name: "event",
  components: {
    EventDetails
  },
  data() {
    return {
      event: {},
      signed: "",
      comments: []
    };
  },
  methods: {
    setData(signed, event, comments) {
      this.event = event;
      this.comments = comments;
      if (signed === 1) this.signed = "true";
      else this.signed = "false";
    }
  },
  beforeRouteEnter(to, from, next) {
    axios
      .all([
        axios.get(`/api/signups/isSigned/${to.params.id}`),
        axios.get(`/api/events/${to.params.id}`),
        axios.get(`/api/comments/${to.params.id}`)
      ])
      .then(
        axios.spread((signed, event, comments) => {
          next(vm => vm.setData(signed.data, event.data, comments.data));
        })
      );
  }
};
</script>