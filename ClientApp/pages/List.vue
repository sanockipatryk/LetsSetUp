<template>
  <div class="events">
    <b-container>
      <b-row>
        <b-col
          class="mb-2 mt-4 col-sm-12 col-12 col-md-12 col-lg-6 col-xl-4"
          v-for="event in paginatedData"
          :key="event.id"
        >
          <b-media class="event">
            <h4
              v-bind:title="event.name"
              class="mt-1"
              @click="view(event)"
            >{{ shorten(event.name) }}</h4>
            <hr>
            <p>
              <b>{{event.category}}</b>
            </p>
            <hr>
            <p v-bind:title="event.city">{{shortenCity(event.city)}}</p>
            <hr>
            <p>
              od:
              <b>{{moment(event.dateStarts).format('DD.MM.YYYY') }}</b>, godz.
              <b>{{moment(event.dateStarts).format('HH:mm')}}</b>
            </p>
            <p>
              do:
              <b>{{moment(event.dateEnds).format('DD.MM.YYYY') }}</b>, godz.
              <b>{{moment(event.dateEnds).format('HH:mm')}}</b>
            </p>
            <hr>
            <p
              v-if="event.status===`Zakończone` && event.usersSigned>=event.peopleNeeded"
            >Wydarzenie zakończone</p>
            <p
              v-else-if="event.status===`Zakończone` && event.usersSigned<event.peopleNeeded"
            >Zbyt mało chętnych osób</p>
            <p
              v-else-if="event.usersSigned < event.peopleNeeded && event.peopleNeeded-event.usersSigned !== 1 && event.peopleNeeded-event.usersSigned !== 2"
            >Potrzeba jeszcze {{event.peopleNeeded-event.usersSigned}} osób!</p>
            <p
              v-else-if="event.peopleNeeded-event.usersSigned === 1"
            >Potrzeba jeszcze {{event.peopleNeeded-event.usersSigned}} osobę!</p>
            <p
              v-else-if="event.peopleNeeded-event.usersSigned === 2"
            >Potrzeba jeszcze {{event.peopleNeeded-event.usersSigned}} osoby!</p>
            <p v-else>Nie ma więcej miejsc!</p>
            <b-progress :max="event.peopleNeeded">
              <b-progress-bar
                v-if="event.usersSigned < event.peopleNeeded"
                :value="event.usersSigned"
                variant="warning"
              ></b-progress-bar>
              <b-progress-bar
                v-if="event.usersSigned >= event.peopleNeeded"
                :value="event.usersSigned"
                variant="success"
              ></b-progress-bar>
              <b-progress-bar
                v-if="event.usersSigned > event.peopleNeeded"
                :value="event.usersSigned-event.peopleNeeded"
                variant="success"
              ></b-progress-bar>
            </b-progress>
            <hr>
            <p>{{event.status}}</p>
            <b-button variant="info" @click="view(event)">Zobacz szczegóły</b-button>
          </b-media>
        </b-col>
      </b-row>
      <div class="pageButtons">
        <b-button @click="prevPage" v-if="pageNumber !== 0">
          <i class="fas fa-backward"></i>
        </b-button>
        <b-button @click="prevPage" v-else disabled>
          <i class="fas fa-backward"></i>
        </b-button>
        <b-button variant="outline-secondary" class="pageNum pageNumButton">{{pageNumber+1}}</b-button>
        <b-button @click="nextPage" v-if="pageNumber*size +size < events.length">
          <i class="fas fa-forward"></i>
        </b-button>
        <b-button @click="nextPage" v-else disabled>
          <i class="fas fa-forward"></i>
        </b-button>
      </div>
    </b-container>
  </div>
</template>


<script>
import EventDetails from "../components/events/Details.vue";
export default {
  name: "event-list",
  components: {
    EventDetails
  },
  props: {
    events: {
      type: Array,
      required: true
    }
  },
  data() {
    return {
      pageNumber: 0,
      size: 6
    };
  },
  computed: {
    pageCount() {
      let l = this.events.length,
        s = this.size;
      return Math.ceil(l / s);
    },
    paginatedData() {
      const start = this.pageNumber * this.size,
        end = start + this.size;
      return this.events.slice(start, end);
    }
  },
  methods: {
    view(event) {
      this.$router.push(`/events/${event.id}/${event.slug}`);
    },
    nextPage() {
      this.pageNumber++;
    },
    prevPage() {
      this.pageNumber--;
    },
    shorten(text) {
      if (text.length > 27) {
        return text.substring(0, 23) + "...";
      } else return text;
    },
    shortenCity(text) {
      if (text.length > 25) {
        return text.substring(0, 25) + "...";
      } else return text;
    }
  }
};
</script>

<style>
header {
  text-align: center;
  font-family: "Roboto", sans-serif;
}
.event {
  box-shadow: 3px 3px 20px rgba(0, 0, 0, 0.75);
  border-radius: 30px;
  background-color: #2b7a78;
  padding: 20px;
  text-align: center;
  box-sizing: border-box;
  height: 100%;
  max-width: 100%;
  height: 470px;
  word-break: break-word;
  -webkit-hyphens: auto;
  -moz-hyphens: auto;
  hyphens: auto;
  text-overflow: wrap;
  margin-bottom: 20px;
}
.event h4 {
  cursor: pointer;
}
.event p {
  margin-top: -8px;
  font-size: 20px;
}
.pageButtons {
  text-align: center;
  margin: 0 0 20px 0;
  line-height: 24px;
}
.pageButtons p {
  display: inline;
  text-align: center;
  line-height: 24px;
  font-size: 24px;

  margin: 0 15px;
}
h4 {
  margin-left: -10px;
  margin-right: -10px;
}
.pageNum.pageNumButton {
  border: none;
  color: white;
  pointer-events: none;
  font-size: 24px;
}
.pageNum.pageNumButton:hover {
  background-color: transparent;
}
@media (max-width: 420px) {
  .event {
    height: 430px;
  }
  .event p {
    font-size: 16px;
  }
  .event h4 {
    font-size: 18px;
  }
}
@media (max-width: 1024px) {
  .event {
    height: 430px;
  }
  .event p {
    font-size: 16px;
  }
  .event h4 {
    font-size: 20px;
  }
}
@media (max-width: 1600px) {
  .event {
    height: 450px;
  }
  .event p {
    font-size: 18px;
  }
  .event h4 {
    font-size: 20px;
  }
}
</style>