<template>
  <b-container class="pt-4">
    <b-button class="backBtn" variant="outline-secondary" @click.prevent="back">
      <i class="fas fa-arrow-left"></i>
      Wróć do wydarzeń
    </b-button>
    <b-row cols="2" class="highRow">
      <b-col class="col-12 col-xl-7 col-lg-7 col-md-12">
        <b-row class="pt-4">
          <b-col class="col-12 mainCol">
            <h3>{{shorten(event.name)}}</h3>
            <p class="mt-3 mb-1">
              <b>Miasto:</b>
              {{shorten(event.city)}}
            </p>
            <p class="mt-1 mb-1" v-if="event.street !== ``">
              <b>Ulica:</b>
              {{shorten(event.street)}}
              <span
                v-if="event.buildingNumber !== ``"
              >{{event.buildingNumber}}</span>
            </p>
            <p class="mt-1 mb-1" v-show="event.objectName !== null || event.objectName !== ``">
              <b>Nazwa obiektu:</b>
              {{shorten(event.objectName)}}
            </p>
            <div class="date date1">
              <label>
                <b>Data rozpoczęcia:</b>
              </label>
              <p class="mt-1 mb-1">
                {{moment(event.dateStarts).format('DD.MM.YYYY') }}, godz.
                {{moment(event.dateStarts).format('HH:mm')}}
              </p>
            </div>
            <div class="date">
              <label>
                <b>Data zakończenia:</b>
              </label>
              <p class="mt-1 mb-1">
                {{moment(event.dateEnds).format('DD.MM.YYYY') }}, godz.
                {{moment(event.dateEnds).format('HH:mm')}}
              </p>
            </div>
            <p class="mt-1 mb-1">
              <b>Liczba miejsc:</b>
              {{ event.peopleNeeded}}
            </p>
            <p class="mt-1 mb-1">
              <b>Liczba chętnych:</b>
              {{event.usersSigned}}
            </p>
            <b-progress class="mt-4" :max="event.peopleNeeded">
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
            <b-row>
              <div class="threeButtons">
                <b-button class="mt-2" variant="primary" @click="showModal1">Zobacz opis wydarzenia</b-button>
                <b-modal ref="myModal1" size="lg" hide-footer title="Opis wydarzenia">
                  <div class="d-block text-center">
                    <h3>{{event.eventDescription}}</h3>
                  </div>
                  <b-btn class="mt-3" variant="outline-primary" block @click="hideModal1">Zamknij</b-btn>
                </b-modal>
                <b-button
                  class="mt-2"
                  variant="primary"
                  @click="showModal2"
                  v-if="event.placeDescription !== ``"
                >Zobacz opis miejsca</b-button>
                <b-modal ref="myModal2" size="lg" hide-footer title="Opis miejsca">
                  <div class="d-block text-center">
                    <h3>{{event.placeDescription}}</h3>
                  </div>
                  <b-btn class="mt-3" variant="outline-primary" block @click="hideModal2">Zamknij</b-btn>
                </b-modal>
                <b-button
                  class="mt-2 webButton"
                  variant="primary"
                  :href="'http://'+event.webPage"
                  v-if="event.webPage!==''"
                >Strona internetowa</b-button>
              </div>
            </b-row>
            <b-row>
              <div class="allbuttons">
                <b-button
                  class="mt-2"
                  variant="danger"
                  @click.prevent="cancel(currentId)"
                  v-if="isAdmin && event.status ===`Nadchodzące` &&  event.isCancelled != true || userId === event.createdBy && event.status ===`Nadchodzące` && event.isCancelled != true"
                >Anuluj wydarzenie</b-button>
                <b-button
                  class="mt-2"
                  variant="info"
                  @click.prevent="view(event)"
                  v-if="userId === event.createdBy && event.status ===`Nadchodzące`  &&  event.isCancelled != true"
                >Edytuj wydarzenie</b-button>
                <b-button
                  class="mt-2"
                  variant="primary"
                  v-if="isAuthenticated && userId !== event.createdBy && signed === 'false' && event.status ==='Nadchodzące' && event.usersSigned < event.peopleNeeded  && event.isCancelled != true"
                  @click.prevent="signUp(event)"
                >Zapisz się</b-button>
                <h3
                  class="mt-2"
                  v-if="!isAuthenticated &&  event.isCancelled != true"
                >Zaloguj się by zapisać się na to wydarzenie.</h3>
                <h3
                  class="mt-2"
                  v-if="isAuthenticated && event.status==`W trakcie` &&  event.isCancelled != true"
                >Wydarzenie już trwa.</h3>
                <h3
                  class="mt-2"
                  v-if="isAuthenticated && event.status==`Zakończone` &&  event.isCancelled != true"
                >To wydarzenie zostało zakończone.</h3>
                <h3 class="mt-2" v-if="event.isCancelled === true">Wydarzenie zostało anulowane.</h3>
                <b-button
                  class="mt-2"
                  variant="warning"
                  v-if="signed === 'true' && event.dateStarts > currentDate  && event.isCancelled != true"
                  @click.prevent="signOut(event)"
                >Wypisz się</b-button>
              </div>
            </b-row>
          </b-col>
        </b-row>
      </b-col>
      <b-col class="col-12 col-xl-5 col-lg-5 col-md-12">
        <b-row class="pt-4">
          <google-map v-if="event" :event="event"></google-map>
        </b-row>
      </b-col>
    </b-row>
    <hr>
    <div>
      <form class="addComm mt-4 mb-4" @submit.prevent="addComment(event)" v-if="isAuthenticated">
        <b-form-group>
          <vue-editor
            :editorToolbar="customToolbar"
            v-model="newComment.content"
            name="editor"
            v-validate="'required|min:5'"
            v-if="event.isCancelled != true"
          ></vue-editor>
          <label v-if="errors.first('editor')" class="invalid-feedback">{{ errors.first('editor') }}</label>
        </b-form-group>
        <b-button
          variant="info"
          @click.prevent="addComment(event)"
          v-if="event.isCancelled != true"
        >Dodaj komentarz</b-button>
      </form>
    </div>
    <div>
      <section class="commentSection" v-for="comment in comms" :key="comment.id">
        <p class="author">
          Autor: {{comment.authorName}}
          <span
            class="creator"
            v-if="comment.isCreator===true"
          >(Organizator)</span>
          <span class="dateSpan">
            <b>{{moment(comment.commentDate).format('DD.MM.YYYY') }}</b>, godz.
            <b>{{moment(comment.commentDate).format('HH:mm:ss')}}</b>
          </span>
        </p>
        <p class="commentContent">
          <span v-html="comment.content"></span>
        </p>
        <b-button
          class="delecteCommentButton"
          v-if="isAdmin"
          variant="danger"
          @click.prevent="deleteComment(comment)"
        >
          <i class="fas fa-times"></i>
          Usuń komentarz
        </b-button>
      </section>
    </div>
  </b-container>
</template>

<script>
import { VueEditor } from "vue2-editor";
import axios from "axios";
import GoogleMap from "../maps/GoogleMap.vue";

export default {
  name: "event-details",
  props: {
    event: {
      type: Object,
      required: true
    },
    signed: String,
    comms: { type: Array, default: [] }
  },
  data() {
    return {
      error: "",
      currentId: "",
      text: "",
      customToolbar: [["bold", "italic", "underline"]],
      newComment: {
        content: ""
      }
    };
  },
  components: {
    GoogleMap,
    VueEditor
  },
  computed: {
    isAdmin() {
      return this.$store.getters.isInRole("Admin");
    },
    currentDate() {
      return new Date().toISOString();
    },
    userId() {
      if (this.$store.state.auth !== null) {
        return this.$store.state.auth.id;
      } else return 0;
    },
    isAuthenticated() {
      return this.$store.getters.isAuthenticated;
    }
  },
  methods: {
    back() {
      this.$router.go(-1);
    },
    showModal1() {
      this.$refs.myModal1.show();
    },
    hideModal1() {
      this.$refs.myModal1.hide();
    },
    showModal2() {
      this.$refs.myModal2.show();
    },
    hideModal2() {
      this.$refs.myModal2.hide();
    },
    view(event) {
      this.$router.push(`/events/${event.id}/${event.slug}/edit`);
    },
    cancel() {
      axios
        .put("/api/events/cancel/" + this.event.id)
        .then(response => {
          axios
            .post("/api/notifications/notifyCanceled/" + this.event.id)
            .then(response => {
              this.$router.push("/events");
            })
            .catch(error => {
              console.log(error.data);
            });
        })
        .catch(error => {
          console.log(error.data);
        });
    },
    signUp(event) {
      axios
        .post("/api/signups/signup/" + this.event.id)
        .then(response => {
          this.$toastr("success", "Zapisano na wydarzenie.");
          this.$router.go();
        })
        .catch(error => {
          console.log(error.data);
          this.$toastr("success", "Nie udało się zapisać na wydarzenie.");
        });
    },
    signOut(event) {
      axios
        .delete("/api/signups/signout/" + this.event.id)
        .then(response => {
          this.$toastr("success", "Wypisano z wydarzenia.");
          this.$router.go();
        })
        .catch(error => {
          console.log(error.data);
        });
    },
    addComment(event) {
      this.$validator.validateAll().then(result => {
        if (result) {
          axios
            .post("/api/comments/addComment/" + this.event.id, this.newComment)
            .then(response => {
              this.$toastr("success", "Komentarz został dodany.");
              axios
                .post(
                  "/api/notifications/notifyNewComment/" +
                    this.event.id +
                    "/" +
                    this.userId
                )
                .then(response => {
                  this.$router.go();
                })
                .catch(error => {
                  console.log(error.data);
                });
            })
            .catch(error => {
              console.log(error.data);
            });
        }
      });
    },
    deleteComment(comment) {
      axios
        .delete("/api/comments/deleteComment/" + comment.id)
        .then(response => {
          this.$router.go();
          this.$toastr("success", "Komentarz został usunięty.");
        })
        .catch(error => {
          console.log(error.data);
        });
    },
    setData(signups) {
      this.signup = signups;
    },
    shorten(text) {
      if (text !== undefined) {
        if (text.length > 44) {
          return text.substring(0, 41) + "...";
        } else return text;
      }
    }
  },
  watch: {
    event(value) {
      this.currentId = value.id;
      console.log(this.signed);
    },
    comms(value) {
      this.comms = value;
    }
  }
};
</script>

<style>
.details {
  padding: 20px;
}
.backBtn {
  position: absolute;
  top: 120px;
  left: 67px;
}
.date {
  display: inline-block;
}
.date.date1 {
  margin-right: 20px;
}
.date p {
  margin-top: 0 !important;
}
.date:last-of-type {
  margin-left: 50px;
}
.btn-outline-secondary {
  color: #feffff;
  border-color: #feffff;
}
.btn-outline-secondary:hover {
  background: #feffff;
  color: #17252a;
}

.commentContent {
  border: 1px solid rgb(204, 204, 204);
  border-top: none;
  padding: 0 10px;
  background-color: #def2f1;
  color: #17252a;
  word-wrap: break-word;
  overflow-wrap: break-word;
  font-size: 18px;
  -webkit-hyphens: auto;
  -moz-hyphens: auto;
  hyphens: auto;
}
.author {
  border: 1px solid rgb(204, 204, 204);
  margin: 0;
  width: 100%;
  padding: 0 10px;
  font-size: 20px;
  background-color: #feffff;
  color: #17252a;
}
.threeButtons {
  text-align: center;
  margin: 0 auto;
}
.threeButtons button,
.webButton {
  display: inline-block;
  width: 100%;
}
.threeButtons button.close {
  width: auto;
}
.creator {
  color: red;
}
.dateSpan {
  float: right;
}
.modal {
  color: #17252a;
}
.quillWrapper {
  box-shadow: 3px 3px 20px rgba(0, 0, 0, 0.75);
}
.ql-toolbar {
  background-color: #feffff;
}
.ql-editor {
  background-color: #def2f1;
  color: #17252a;
}
.addComm {
  text-align: center;
}
.commentSection {
  position: relative;
  box-shadow: 3px 3px 20px rgba(0, 0, 0, 0.75);
}
.delecteCommentButton {
  position: absolute;
  right: -180px;
  top: 0;
}
.allbuttons {
  text-align: center;
  margin: 0 auto;
}
@media (max-width: 1600px) {
  .backBtn {
    top: 90px;
    left: 50px;
  }
  .highRow {
    margin-top: 30px;
  }
}
@media (max-width: 1024px) {
  .backBtn {
    top: 90px;
    left: 50px;
  }
  .highRow {
    margin-top: 30px;
  }
}
@media (max-width: 768px) {
  .mainCol h3 {
    font-size: 26px;
  }
  .mainCol p {
    font-size: 20px;
  }
  .backBtn {
    visibility: hidden;
  }
  .allbuttons {
    text-align: center;
  }
  .allbuttons button {
    width: 64%;
  }
}
@media (max-width: 420px) {
  .mainCol h3 {
    font-size: 22px;
  }
  .mainCol p {
    font-size: 16px;
  }
  .backBtn {
    visibility: hidden;
  }
  .allbuttons {
    text-align: center;
  }
  .allbuttons button {
    width: 64%;
  }
}
</style>