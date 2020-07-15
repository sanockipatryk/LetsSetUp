<template>
  <div class="google-map" :id="mapName"></div>
</template>
<script>
export default {
  name: "google-map",
  props: {
    event: { type: Object, required: true }
  },
  data: function() {
    return {
      mapName: this.name + "-map",
      markerCoordinates: [
        {
          latitude: 0,
          longitude: 0
        }
      ],
      latLong: {
        latitute2: "",
        longitude2: ""
      },
      bounds: null,
      markers: [],
      geocoder: new google.maps.Geocoder(),
      map: null
    };
  },
  mounted: function() {
    const element = document.getElementById(this.mapName);
    const mapCentre = this.markerCoordinates;
    const options = {
      zoom: 17,
      center: new google.maps.LatLng(mapCentre.latitude, mapCentre.longitude)
    };
    this.map = new google.maps.Map(element, options);
  },
  methods: {
    decodeLatLong(geocoder, resultsMap, newAdr) {
      let latLong = this.latLong;
      return new Promise(function(resolve, reject) {
        geocoder.geocode({ address: newAdr }, function(results, status) {
          if (status === google.maps.GeocoderStatus.OK) {
            resultsMap.setCenter(results[0].geometry.location);
            var marker = new google.maps.Marker({
              title: "Miejsce wydarzenia",
              map: resultsMap,
              position: results[0].geometry.location
            });
          } else {
            reject(status);
          }
        });
      });
    }
  },
  watch: {
    event(value) {
      let address = "";
      address = value.city;
      if (value.objectName !== null) address = address + " " + value.objectName;
      if (value.street !== null) {
        address = address + " " + value.street;
        if (value.buildingNumber !== null)
          address = address + " " + value.buildingNumber;
      }
      this.decodeLatLong(this.geocoder, this.map, address);
    }
  }
};
</script>

<style scoped>
.google-map {
  width: 480px;
  height: 500px;
  margin: 0 16px;
  text-align: left;
  background: gray;
  box-shadow: 3px 3px 20px rgba(0, 0, 0, 0.75);
}
@media (max-width: 850px) {
  .google-map {
    width: 100%;
  }
}
</style>