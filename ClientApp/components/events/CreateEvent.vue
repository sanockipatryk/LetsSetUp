<template>
  <div class="wrap">
    <div class="clearfix">
      <h3 class="float-left">Dodaj nowe wydarzenie</h3>
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
        <b-button class="float-right" variant="primary" @click.prevent="save">Dodaj wydarzenie</b-button>
      </div>
    </form>
  </div>
</template>

<script>
import axios from "axios";
import FormInput from "../../components/shared/FormInput.vue";
import FormTextArea from "../../components/shared/FormTextArea.vue";

export default {
  name: "create-event",
  components: {
    FormInput,
    FormTextArea
  },
  data() {
    return {
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
    setData(data) {
      this.categories = data.categories;
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
                  .post("/api/events", this.event)
                  .then(response => {
                    this.$toastr("success", "Wydarzenie zostało utworzone.");
                    this.$router.push("/events");
                  })
                  .catch(error => {
                    this.$toastr("error", "Nie udało się utworzyć wydarzenia.");
                    console.log(error.data);
                  });
              }
            });
          }
        }
      });
    }
  },
  beforeRouteEnter(to, from, next) {
    const vm = this;
    axios.get("/api/filters").then(response => {
      next(vm => vm.setData(response.data));
    });
  }
};
</script>

<style>
.wrap {
  width: 70%;
  margin: 20px auto;
  font-size: 0;
}
label {
  font-size: 18px;
}
.smallInput {
  width: 50%;
  display: inline-block;
  box-sizing: border-box;
}
.cityInput {
  width: 45%;
  display: inline-block;
}
.streetInput {
  width: 45%;
  display: inline-block;
}
.buildingInput {
  width: 10%;
  display: inline-block;
}
.selectForm option {
  font-size: 20px;
}
.invalid-feedback {
  display: block;
  color: rgb(194, 24, 24);
  font-size: 18px;
}
p.legend {
  color: #feffff;
}
@media (max-width: 420px) {
  .cityInput {
    width: 100%;
    display: block;
  }
  .streetInput {
    width: 100%;
    display: block;
  }
  .buildingInput {
    width: 100%;
    display: block;
  }
  .smallInput {
    width: 100%;
  }
  .eventbackBtn {
    visibility: hidden;
  }
}
</style>