<template>
  <b-container fluid class="page">
    <b-row>
      <b-col
        class="col-xl-3 col-md-6 col-lg-4 col-12 col-sm-12 order-lg-1 order-md-1 order-sm-2 order-2"
      >
        <filters v-if="filters.categories.length" :filters="filters"/>
      </b-col>
      <b-col class="col-xl-9 col-md-6 col-lg-8 col-12 col-sm-12 order-lg-2 order-md-2 order-sm-1">
        <div class="mt-4 clearfix">
          <event-sort class="float-right"/>
        </div>
        <event-list v-if="events.length" :events="sortedEvents"/>
      </b-col>
    </b-row>
  </b-container>
</template>
<script>
import EventList from "./List.vue";
import axios from "axios";
import Filters from "../components/catalogue/Filters.vue";
import EventSort from "../components/catalogue/EventSort.vue";

export default {
  name: "catalogue",
  components: {
    EventList,
    EventSort,
    Filters
  },
  data() {
    return {
      events: [],
      filters: {
        categories: [],
        statuses: [],
        dateStarts: "",
        dateEnds: ""
      }
    };
  },
  computed: {
    sort() {
      return this.$route.query.sort || 0;
    },
    sortedEvents() {
      switch (this.sort) {
        case 1:
          return this.events.sort((a, b) => {
            if (b.dateStarts > a.dateStarts) return 1;
            else if (b.dateStarts === a.dateStarts) return 0;
            else return -1;
          });
          break;
        case 2:
          return this.events.sort((a, b) => {
            if (b.peopleNeeded - b.usersSigned > a.peopleNeeded - a.usersSigned)
              return 1;
            else if (b.usersSigned === a.usersSigned) return 0;
            else return -1;
          });
          break;
        case 3:
          return this.events.sort((a, b) => {
            if (a.peopleNeeded - a.usersSigned > b.peopleNeeded - b.usersSigned)
              return 1;
            else if (a.usersSigned === b.usersSigned) return 0;
            else return -1;
          });
          break;
        case 4:
          return this.events.sort((a, b) => {
            if (a.name > b.name) return 1;
            else if (a.name === b.name) return 0;
            else return -1;
          });
          break;
        case 5:
          return this.events.sort((a, b) => {
            if (b.name > a.name) return 1;
            else if (b.name === a.name) return 0;
            else return -1;
          });
          break;
        default:
          return this.events.sort((a, b) => {
            if (a.dateStarts > b.dateStarts) return 1;
            else if (a.dateStarts === b.dateStarts) return 0;
            else return -1;
          });
          break;
      }
    }
  },
  methods: {
    setData(events, filters) {
      this.events = events;
      this.filters = filters;
    }
  },

  beforeRouteEnter(to, from, next) {
    axios
      .all([
        axios.get("/api/events", { params: to.query }),
        axios.get("/api/filters")
      ])
      .then(
        axios.spread((events, filters) => {
          next(vm => vm.setData(events.data, filters.data));
        })
      );
  },
  beforeRouteUpdate(to, from, next) {
    axios.get("/api/events", { params: to.query }).then(response => {
      this.events = response.data;
      next();
    });
  }
};
</script>

