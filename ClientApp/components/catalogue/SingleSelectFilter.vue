<template>
  <b-row>
    <b-col cols="6">
      <div :class="{ 'filter-item': true,
'active': itemSelected === ''}" @click="clearSt">Wszystkie</div>
    </b-col>
    <b-col cols="6" v-for="item in items" :key="item">
      <div
        :class="{ 'filter-item': true, 'active':(item === itemSelected) }"
        @click="filter(item)"
      >{{ item }}</div>
    </b-col>
  </b-row>
</template>

<script>
export default {
  name: "single-select-filter",
  props: {
    queryKey: {
      type: String,
      required: true
    },
    items: {
      type: Array,
      required: true
    }
  },
  data() {
    return {
      itemSelected: ""
    };
  },
  computed: {
    selected() {}
  },
  methods: {
    clearSt() {
      let query = Object.assign({}, this.$route.query);
      delete query.statuses;
      this.itemSelected = "";
      this.$router.push({ query: query });
    },
    filter(item) {
      if (this.itemSelected === "" || this.itemSelected !== item) {
        let query = Object.assign({}, this.$route.query);
        query.statuses = item;
        this.itemSelected = item;
        this.$router.push({ query: query });
      } else {
        let query = Object.assign({}, this.$route.query);
        delete query.statuses;
        this.itemSelected = "";
        this.$router.push({ query: query });
      }
    }
  },
  created() {
    this.itemSelected = "Nadchodzące";
    let query = Object.assign({}, this.$route.query);
    query.statuses = this.itemSelected;
    this.$router.push({ query: query });
  }
};
</script>

<style scoped>
.filter-item {
  margin: 10px 0;
  border: 1px solid #17252a;
  font-size: 18px;
  color: #17252a;
  border-radius: 5px;
  padding: 10px;
  text-align: center;
  cursor: pointer;
}
.filter-item.active {
  font-weight: bold;
  color: #feffff;
  border-color: #feffff;
}
@media (max-width: 1600px) {
  .filter-item {
    font-size: 16px;
  }
}
@media (max-width: 1024px) {
  .filter-item {
    font-size: 14px;
  }
}
</style>
