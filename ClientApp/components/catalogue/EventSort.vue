<template>
  <div>
    <span class="label mr-2">Sortuj:</span>
    <b-dropdown id="myDropdown" class="dropdown" right :text="items[selected]">
      <b-dropdown-item v-for="(item, index) in items" :key="index" @click="select(index)">{{ item }}</b-dropdown-item>
    </b-dropdown>
  </div>
</template>

<script>
export default {
  data() {
    return {
      items: [
        "Data rozpoczęcia (Najwcześniej)",
        "Data rozpoczęcia (Najpóźniej)",
        "Liczba chętnych (Najmniejsza)",
        "Liczba chętnych (Największa)",
        "Nazwa (A - Z)",
        "Nazwa (Z - A)"
      ]
    };
  },
  computed: {
    selected() {
      return this.$route.query.sort || 0;
    }
  },
  methods: {
    select(index) {
      if (index === 0) {
        let query = Object.assign({}, this.$route.query);
        delete query.sort;
        this.$router.push({ query: query });
      } else {
        let query = Object.assign({}, this.$route.query);
        query.sort = index;
        this.$router.push({ query: query });
      }
    }
  }
};
</script>

<style scoped>
.label,
.dropdown {
  vertical-align: middle;
}
</style>

