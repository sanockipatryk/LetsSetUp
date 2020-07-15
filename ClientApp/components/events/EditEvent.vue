<template>
  <div class="wrap">
    <div class="clearfix">
      <h3 class="float-left">Edytuj wydarzenie</h3>
    </div>
    <form class="mt-4 mb-4" @submit.prevent="save">
      <b-form-group>
        <form-input
          label="Nazwa wydarzenia*"
          name="name"
          :error="errors.first('name')"
          v-model="event.name"
          v-validate="'required|min:5|max:250'"
        />
        <form-input
          class="smallInput cityInput"
          type="text"
          label="Miasto*"
          name="city"
          :error="errors.first('city')"
          v-model="event.city"
          v-validate="'required|max:50'"
        />
        <form-input
          class="smallInput streetInput"
          type="text"
          label="Ulica"
          name="street"
          :error="errors.first('street')"
          v-model="event.street"
          v-validate="'max:100'"
        />
        <form-input
          class="buildingInput"
          type="number"
          label="Numer budynku"
          name="buildingNumber"
          :error="errors.first('buildingNumber')"
          v-model="event.buildingNumber"
          v-validate="'numeric'"
        />
        <form-input
          type="text"
          label="Nazwa obiektu"
          name="objectName"
          :error="errors.first('objectName')"
          v-model="event.objectName"
          v-validate="'max:50'"
        />
        <form-input
          class="smallInput"
          type="datetime-local"
          label="Data rozpoczęcia*"
          name="dateStarts"
          :error="errors.first('dateStarts')"
          v-model="event.dateStarts"
          v-validate="'required|minDate'"
        />
        <form-input
          class="smallInput"
          type="datetime-local"
          label="Data zakończenia*"
          name="dateEnds"
          :error="errors.first('dateEnds')"
          v-model="event.dateEnds"
          v-validate="'required|minDate'"
        />
        <label
          v-if="event.dateEnds <= event.dateStarts && event.dateStarts !== '' && event.dateEnds !== '' && errors.first('dateEnds') == null"
          class="invalid-feedback"
        >Data zakończenia musi być późniejsza</label>
        <form-input
          type="number"
          label="Liczba miejsc*"
          name="peopleNeeded"
          :error="errors.first('peopleNeeded')"
          v-model="event.peopleNeeded"
          v-validate="'required|numeric|min_value:1'"
        />
        <form-text-area
          type="text"
          label="Opis wydarzenia*"
          name="eventDescription"
          :error="errors.first('eventDescription')"
          v-model="event.eventDescription"
          v-validate="'required|min:20|max:500'"
        />
        <form-text-area
          type="text"
          label="Opis miejsca"
          name="placeDescription"
          :error="errors.first('placeDescription')"
          v-model="event.placeDescription"
          v-validate="'min:20|max:500'"
        />
        <form-input
          type="text"
          label="Strona internetowa"
          name="webPage"
          :error="errors.first('webPage')"
          v-model="event.webPage"
          v-validate="'max:100|url'"
        />
        <label>Kategoria*</label>
        <b-form-select
          class="selectForm"
          v-model="event.category"
          :options="categories"
          name="category"
          v-validate="'required'"
          :value="event.category"
        />
        <p v-if="errors.first('category')" class="invalid-feedback">{{ errors.first('category') }}</p>
        <p class="legend mt-4">* - Pole wymagane</p>
      </b-form-group>
      <div class="clearfix">
        <b-button class="float-left eventbackBtn" variant="outline-secondary" to="/events">
          <i class="fas fa-arrow-left"></i>
          Wróć do wydarzeń
        </b-button>
        <b-button class="float-right" variant="primary" @click.prevent="save">Edytuj wydarzenie</b-button>
      </div>
    </form>
  </div>
</template>

<script>
import axios from "axios";
import FormInput from "../../components/shared/FormInput.vue";
import FormTextArea from "../../components/shared/FormTextArea.vue";

export default {
  name: "edit-event",
  components: {
    FormInput,
    FormTextArea
  },
  data() {
    return {
      eventId: "",
      createdBy: "",
      event: {
        name: "",
        city: "",
        street: "",
        objectName: "",
        buildingNumber: "",
        dateStarts: "",
        dateEnds: "",
        peopleNeeded: "",
        eventDescription: "",
        placeDescription: "",
        webPage: "",
        category: ""
      },
      categories: []
    };
  },
  methods: {
    setEvent(event, filters) {
      var tzoffset = new Date().getTimezoneOffset() * 60000;
      var localISOTime = new Date(Date.now() - tzoffset)
        .toISOString()
        .slice(0, -1);
      if (this.userId != event.createdBy || event.dateStarts < localISOTime) {
        this.$router.push("/events");
      } else {
        this.categories = filters.categories;
        this.eventId = event.id;
        this.createdBy = event.createdBy;
        this.isOwner;
        this.event.name = event.name;
        this.event.city = event.city;
        if (event.street != null) this.event.street = event.street;
        if (event.objectName != null) this.event.objectName = event.objectName;
        if (event.buildingNumber != null)
          this.event.buildingNumber = event.buildingNumber;
        this.event.dateStarts = event.dateStarts;
        this.event.dateEnds = event.dateEnds;
        this.event.peopleNeeded = new String(event.peopleNeeded);
        if (event.eventDescription != null)
          this.event.eventDescription = event.eventDescription;
        if (event.placeDescription != null)
          this.event.placeDescription = event.placeDescription;
        if (event.webPage != null) this.event.webPage = event.webPage;
        this.event.category = event.category;
      }
    },
    save() {
      this.$validator.validateAll().then(result => {
        if (result) {
          if (
            this.event.dateStarts !== "" &&
            this.event.dateEnds !== "" &&
            this.event.dateEnds > this.event.dateStarts
          ) {
            this.$validator.validateAll().then(result => {
              if (result) {
                axios
                  .put("/api/events/edit/" + this.eventId, this.event)
                  .then(response => {
                    this.$toastr("success", "Wydarzenie zostało zmienione.");
                    axios
                      .post("/api/notifications/notifyEdited/" + this.eventId)
                      .then(response => {
                        this.$router.push("/events");
                      })
                      .catch(error => {
                        console.log(error.data);
                      });
                  })
                  .catch(error => {
                    console.log(error.data);
                    this.$toastr("error", "Nie udało się zmienić wydarzenia.");
                  });
              }
            });
          }
        }
      });
    }
  },
  computed: {
    userId() {
      if (this.$store.state.auth !== null) {
        return this.$store.state.auth.id;
      } else return 0;
    },
    isOwner() {
      if (this.$store.state.auth.id !== this.createdBy) {
        this.$router.push("/events");
        return false;
      } else return true;
    }
  },
  beforeRouteEnter(to, from, next) {
    const vm = this;
    axios
      .all([
        axios.get(`/api/events/${to.params.id}`),
        axios.get("/api/filters")
      ])
      .then(
        axios.spread((events, filters) => {
          next(vm => vm.setEvent(events.data, filters.data));
        })
      );
  }
};
</script>
