<template>
  <div class="filters mb-4">
    <div class="filtersHeader">
      <b-btn variant="outline-secondary" @click.prevent="reset">
        <i class="fas fa-sync mr-2"></i> Zresetuj filtry
      </b-btn>
    </div>
    <b-list-group class="mt-4">
      <filter-accordion v-if="isAuthenticated && isCustomer">
        <span slot="header">Moje wydarzenia</span>
        <div class="filter-item" slot="body">
          <b-form-checkbox
            class="checkboxForm"
            v-model="mine"
            value="true"
            unchecked-value="false"
          >Utworzone przeze mnie</b-form-checkbox>
        </div>
      </filter-accordion>
      <filter-accordion>
        <span slot="header">Nazwa wydarzenia</span>
        <div class="filter-item input" slot="body">
          <b-form-input :value="name" type="text" @input="updateName"></b-form-input>
        </div>
      </filter-accordion>
      <filter-accordion>
        <span slot="header">Miasto</span>
        <div class="filter-item input" slot="body">
          <b-form-input :value="city" type="text" @input="updateCity"></b-form-input>
        </div>
      </filter-accordion>
      <filter-accordion>
        <span slot="header">Kategoria</span>
        <multi-select-filter slot="body" query-key="categories" :items="filters.categories"/>
      </filter-accordion>
      <filter-accordion>
        <span slot="header">Status</span>
        <single-select-filter
          slot="body"
          query-key="statuses"
          ref="singleSelectFilter"
          :items="filters.statuses"
        />
      </filter-accordion>
      <filter-accordion>
        <span slot="header">Data rozpoczęcia</span>
        <div class="filter-item input" slot="body">
          <b-form-input :value="date1" type="datetime-local" @input="updateDate1"></b-form-input>
        </div>
      </filter-accordion>
      <filter-accordion>
        <span slot="header">Data zakończenia</span>
        <div class="filter-item input" slot="body">
          <b-form-input :value="date2" type="datetime-local" @input="updateDate2"></b-form-input>
        </div>
      </filter-accordion>
    </b-list-group>
  </div>
</template>

<script>
import FilterAccordion from "./FilterAccordion.vue";
import MultiSelectFilter from "./MultiSelectFilter.vue";
import SingleSelectFilter from "./SingleSelectFilter.vue";
import _ from "lodash";
export default {
  name: "filters",
  props: {
    filters: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      date1: "",
      date2: "",
      name: "",
      city: "",
      mine: false
    };
  },
  components: {
    FilterAccordion,
    MultiSelectFilter,
    SingleSelectFilter
  },
  computed: {
    categories() {
      return this.$route.query.categories || "";
    },
    statuses() {
      return this.$route.query.statuses || "";
    },
    isAuthenticated() {
      return this.$store.getters.isAuthenticated;
    },
    isCustomer() {
      return (
        this.$store.getters.isInRole("Customer") ||
        !this.$store.getters.isAuthenticated
      );
    }
  },
  methods: {
    updateDate1(newdate) {
      this.date1 = newdate;
      this.filterDates(this.date1, this.date2);
    },
    updateDate2(value) {
      this.date2 = value;
      this.filterDates(this.date1, this.date2);
    },
    updateName(value) {
      this.name = value;
      this.filterName(this.name);
    },
    updateCity(value) {
      this.city = value;
      this.filterCity(this.city);
    },
    reset() {
      this.$router.push({ query: {} });
      this.date1 = "";
      this.date2 = "";
      this.name = "";
      this.city = "";
      this.mine = false;
      this.$refs.singleSelectFilter.clearSt();
    },
    clearCategories() {
      if (this.categories.length) {
        let query = Object.assign({}, this.$route.query);
        delete query.categories;
        this.$router.push({ query: query });
      }
    },
    filterCategory(category) {
      let query = Object.assign({}, this.$route.query);
      let split = query.categories ? query.categories.split("|") : [];
      if (split.indexOf(category) > -1) {
        let index = split.indexOf(category);
        split.splice(index, 1);
      } else {
        split.push(category);
      }
      if (split.length) {
        let joined = split.join("|");
        query.categories = joined;
      } else {
        delete query.categories;
      }
      this.$router.push({ query: query });
    },
    filterDates: _.debounce(function(date1, date2) {
      let query = Object.assign({}, this.$route.query);
      query.dateStarts = date1;
      query.dateEnds = date2;
      this.$router.push({ query: query });
    }, 500),
    filterName: _.debounce(function() {
      let query = Object.assign({}, this.$route.query);
      query.name = this.name;
      this.$router.push({ query: query });
    }, 500),
    filterCity: _.debounce(function() {
      let query = Object.assign({}, this.$route.query);
      query.city = this.city;
      this.$router.push({ query: query });
    }, 500)
  },
  watch: {
    mine(value) {
      this.mine = value;
      if (this.mine) {
        let query = Object.assign({}, this.$route.query);
        query.mine = this.mine;
        this.$router.push({ query: query });
      }
    }
  }
};
</script>

<style scoped>
.filters {
  margin-top: 47px;
}
.filter-item {
  margin: 10px 0;
  border: 1px solid #edf5e1;
  border-radius: 10px;
  padding: 10px;
  color: black;
  text-align: center;
  cursor: pointer;
}
.filter-item.active {
  font-weight: bold;
  color: #2b7a78;
  border-color: #2b7a78;
}
.input {
  cursor: default;
}
.btn-outline-secondary {
  color: #feffff;
  border-color: #feffff;
  margin-bottom: 0;
}
.btn-outline-secondary:hover {
  background: #feffff;
  color: #17252a;
}
.filtersHeader {
  text-align: center;
  margin-top: -20px;
}
span {
  color: #feffff;
  font-size: 20px;
  font-weight: 400;
}
.datePicker {
  padding: 35px 0 10px 10px;
}

.list-group {
  box-shadow: 3px 3px 20px rgba(0, 0, 0, 0.75);
  border-radius: 15px;
}
.list-group-item:first-child {
  border-top-left-radius: 15px;
  border-top-right-radius: 15px;
}

.list-group-item:last-child {
  border-bottom-left-radius: 15px;
  border-bottom-right-radius: 15px;
}
.checkboxForm {
  color: #feffff;
}
@media (max-width: 1600px) {
  span {
    font-size: 20px;
  }
}

@media (max-width: 1024px) {
  span {
    font-size: 16px;
  }
  .filter-item {
    font-size: 12px;
  }
}
</style>