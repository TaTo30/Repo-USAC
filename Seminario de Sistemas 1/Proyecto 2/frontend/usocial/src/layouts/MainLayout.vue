<template>
  <q-layout view="hHh lpR fFf">

    <q-header  class="bg-indigo-6 text-white">
      <q-toolbar>
        <q-toolbar-title>
          <q-avatar>
            <q-icon name="pages" size="lg" />
          </q-avatar>
          U-Social
          <q-badge color="white" text-color="indigo-10" label="G.24" />
        </q-toolbar-title>
        <q-space />
        <q-btn unelevated round icon="logout" to="/" />
      </q-toolbar>
    </q-header>

    <q-drawer class="bg-grey-10" show-if-above :width="500" side="left" elevated>
      <UserInfo />
      <q-inner-loading dark :showing="loadingu">
        <q-spinner size="50px" color="white" />
      </q-inner-loading>
    </q-drawer>

    <q-drawer class="bg-grey-10" show-if-above :width="500" side="right" elevated>
      <Amigos/>
      <q-inner-loading dark :showing="loadinga">
        <q-spinner size="50px" color="white" />
      </q-inner-loading>
    </q-drawer>

    <q-page-container class="bg-grey-9">
      <q-scroll-area :style="{height: $q.screen.height - 50 + 'px'}">
        <Publicaciones />
      </q-scroll-area>
      <q-inner-loading dark :showing="loading">
        <q-spinner size="50px" color="white" />
      </q-inner-loading>
    </q-page-container>

  </q-layout>
</template>

<script>
import { ref, computed } from 'vue'
import { useStore } from 'vuex'

import UserInfo from "../components/UserInfo.vue";
import Amigos from "../components/Amigos.vue"
import Publicaciones from "../components/Publicaciones.vue"

export default {
  components: {
    UserInfo,
    Amigos,
    Publicaciones
  },

  setup(){
    return {
      loading: computed(() => useStore().state.publicaciones.loading),
      loadingu: computed(() => useStore().state.usuario.loading),
      loadinga: computed(() => useStore().state.amigo.loading)
    }
  }

}
</script>