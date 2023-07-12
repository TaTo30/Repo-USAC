<template>
  <div class="q-pa-sm">
      <div class="row items-start justify-between">
          <div class="text-h6 text-grey-4">
              <q-item>
                  <q-item-section avatar>
                      <q-icon name="people" />
                  </q-item-section>
                  <q-item-section>
                      <q-item-label>Amigos</q-item-label>
                  </q-item-section>
              </q-item>
          </div>
          <div>
              <q-btn color="white" icon="refresh" round unelevated flat @click="obtenerAmigos" />
          </div>
      </div>
      <div class="q-pa-sm">
          <div v-if="amigos.length > 0">
              <q-item dark class="bg-grey-9 q-mb-xs rounded-borders" v-for="(amigo, index) in amigos" :key="index">
                <q-item-section top avatar>
                    <q-avatar>
                        <img :src="amigo.image" />
                    </q-avatar>
                </q-item-section>
                <q-item-section>
                    <q-item-label>{{amigo.nickname}}</q-item-label>
                </q-item-section>
                <q-item-section side>
                    <q-btn v-if="amigo.bot === 1" @click="opencovidchat = true" color="red" icon="announcement" round unelevated flat />
                    <q-btn v-else @click="amigochat = amigo; openchat = true" color="white" icon="chat" round unelevated flat />
                </q-item-section>
            </q-item>
          </div>
          <div v-else>
              <q-item dark>                  
                  <q-item-section top side>
                      <q-icon name="mood_bad" />
                  </q-item-section>
                  <q-item-section>
                      <q-item-label><i>¡No tienes amistades!</i></q-item-label>
                  </q-item-section>
              </q-item>
          </div>
      </div>
  </div>
  <div class="q-pa-sm">
      <div class="text-h6 text-grey-4">
          <q-item>
              <q-item-section avatar>
                  <q-icon name="person_add" />
              </q-item-section>
              <q-item-section>
                  <q-item-label>Solicitudes de amistad</q-item-label>
              </q-item-section>
          </q-item>
      </div>
      <div class="q-pa-sm">
          <div v-if="solicitudes.length > 0">
            <q-item dark class="bg-grey-9 q-mb-xs rounded-borders" v-for="(sol, index) in solicitudes" :key="index">
                <q-item-section top avatar>
                    <q-avatar>
                        <img :src="sol.image" />
                    </q-avatar>
                </q-item-section>
                <q-item-section>
                    <q-item-label>{{sol.nickname}}</q-item-label>
                    <q-item-label caption>
                        <q-btn color="info" flat label="Aceptar" @click="procesarSolicitud({friend: sol.nickname, type: 1})" />
                        <q-btn color="red" flat label="Rechazar" @click="procesarSolicitud({friend: sol.nickname, type: 0})" />
                    </q-item-label>
                </q-item-section>
            </q-item>
          </div>
          <div v-else>
              <q-item dark>                  
                  <q-item-section top side>
                      <q-icon name="celebration" />
                  </q-item-section>
                  <q-item-section>
                      <q-item-label><i>No tienes solicitudes pendiente</i></q-item-label>
                  </q-item-section>
              </q-item>
          </div>
      </div>
  </div>
  <div class="q-pa-sm">
      <div class="text-h6 text-grey-4">
          <q-item>
              <q-item-section avatar>
                  <q-icon name="group_add" />
              </q-item-section>
              <q-item-section>
                  <q-item-label>Agregar amigos</q-item-label>
              </q-item-section>
          </q-item>
      </div>
      <div class="q-pa-sm">
          <div v-if="noAmigos.length > 0">
              <q-item dark class="bg-grey-9 q-mb-xs rounded-borders" v-for="(noamigo, index) in noAmigos" :key="index">
                <q-item-section top avatar>
                    <q-avatar>
                        <img :src="noamigo.image" />
                    </q-avatar>
                </q-item-section>
                <q-item-section>
                    <q-item-label>{{noamigo.nickname}}</q-item-label>
                </q-item-section>
                <q-item-section side>
                    <q-btn color="positive" flat label="Agregar a amigos" @click="enviarSolicitud(noamigo.nickname)" />
                </q-item-section>
            </q-item>
          </div>
          <div v-else>
              <q-item dark>                  
                  <q-item-section top side>
                      <q-icon name="child_care" />
                  </q-item-section>
                  <q-item-section>
                      <q-item-label><i>¡No hay personas en la plataforma!</i></q-item-label>
                  </q-item-section>
              </q-item>
          </div>
      </div>
  </div>
  <q-dialog v-model="openchat" >
      <UserChat :friend="amigochat" />
  </q-dialog>
    <q-dialog v-model="opencovidchat">
      <CovidChat />
  </q-dialog>
</template>

<script>
import { ref } from "vue";
import { useStore } from "vuex";
import { useQuasar } from 'quasar';

import UserChat from '../components/UserChat.vue'
import CovidChat from '../components/CovidChat.vue'

export default {
    components: {
        UserChat,
        CovidChat
    },
    setup() {
        const store = useStore()
        const $q = useQuasar()


        const amigos = ref([])
        const noAmigos = ref([])
        const solicitudes = ref([])

        const amigochat = ref('')

        const obtenerAmigos = () => {
            store.commit('amigo/setLoading', true)
            store.dispatch('amigo/obtenerAmigos')
            .then(data => {
                amigos.value = data.filter((value) => {
                    if (value.is_friend === 1)  return value
                })
                noAmigos.value = data.filter(value => {
                    if (value.is_friend === -1)  return value
                })
                solicitudes.value = data.filter(value => {
                    if (value.is_friend === 0)  return value
                })
            }).finally(() => store.commit('amigo/setLoading', false))
        }

        const enviarSolicitud = (value) => {
            store.dispatch('amigo/solicitarAmistad', {
                nickname: store.state.usuario.nickname,
                friend: value
            }).then(data => {
                $q.notify({
                    position: 'bottom-right',
                    message: 'Solicitud de amistad enviada',
                    type: 'info',
                    color: 'primary'
                })
                obtenerAmigos()
            })
        }

        const procesarSolicitud = (value) => {
            store.dispatch('amigo/aceptarAmistad', {
                nickname: store.state.usuario.nickname,
                friend: value.friend,
                accept: value.type
            }).then(data => {
                $q.notify({
                    position: 'bottom-right',
                    message: 'Solicitud de amistad procesada',
                    type: 'info',
                    color: 'primary'
                })
                obtenerAmigos()
            })
        }

        obtenerAmigos()
        return {
            store,
            amigos,
            amigochat,
            noAmigos,
            solicitudes,
            enviarSolicitud,
            procesarSolicitud,
            obtenerAmigos,
            openchat: ref(false),
            opencovidchat: ref(false)
        }
    }

}
</script>

<style>

</style>