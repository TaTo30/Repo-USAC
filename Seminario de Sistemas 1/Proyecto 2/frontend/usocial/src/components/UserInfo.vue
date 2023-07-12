<template>
  <div class="q-pa-md">
      <div>        
        <div class="row items-center justify-center " >
          <div style="width: 125px" class="relative-position">
            <img width="125" height="125" style="border-radius: 100%" :src="userinfo.usuario.image" >
            <q-btn
              round
              color="positive"
              icon="add_a_photo"
              class="absolute-bottom-right"
              @click="takepic = true"
            />
          </div>
        </div>
        <div class="row items-center justify-center q-mt-sm text-grey-5 text-h4" >
            {{userinfo.usuario.nickname}}
        </div>
        <div class="row items-center justify-center text-grey-3" >
            {{userinfo.usuario.email}}
        </div>
        <div class="text-h6 q-mt-md text-grey-3">Actualizar Datos</div>
        <div>
          <q-item>
            <q-item-section>
              <q-toggle color="white" class="text-white" dark v-model="userinfo.usuario.bot" label="Usuario tipo BOT" />
            </q-item-section>
          </q-item>          
        </div>
        <div>
          <q-item>
            <q-item-section>
              <q-input dark outlined dense color="indigo-10" v-model="userinfo.usuario.name" input-class=" q-py-xs" type="text" label="Nombre(s) y Apellido(s)" />
            </q-item-section>
          </q-item>          
        </div>
      </div>      
      <q-separator spaced />
      <div>
        <div>
          <q-item>
            <q-item-section>
              <q-input dark outlined dense  v-model="newpass" type="password" label="Contraseña" />
            </q-item-section>
            <q-item-section side>
                <q-btn :disable="!newpass" color="positive" icon="save" label="Actualizar" @click="editarUsuario"  />
            </q-item-section>
          </q-item>        
        </div>
      </div>
  </div>
  <q-dialog v-model="takepic" >
       <TomarFoto @picture="setPicture" />
   </q-dialog>
</template>

<script>
import { ref } from "vue";
import { useStore } from "vuex";
import { useQuasar } from 'quasar'

import TomarFoto from "../components/TomarFoto.vue";

export default {
    components: {
        TomarFoto
    },
    setup() {
        const $store = useStore()
        const $q = useQuasar()

        const userinfo = ref({
            usuario: {
                image: '',
                nickname: '',
                name: '',
                email: '',
                password: '',
                bot: false
            }
        })
        const newpass = ref('')

        const getData = () => {
            $store.commit('usuario/setLoading', true)
            $store.dispatch('usuario/userData')
            .then(data => {
                userinfo.value = data
                userinfo.value.usuario.bot = data.usuario.bot == 0? false: true
            }).finally(() => $store.commit('usuario/setLoading', false))
        }

        const setPicture = (value) => {
            userinfo.value.usuario.image = value
        } 

        const editarUsuario = () => {
            $store.commit('usuario/setLoading', true)
            $store.dispatch('usuario/editarUser', {
                password: newpass.value,
                name: userinfo.value.usuario.name,
                nickname: userinfo.value.usuario.nickname,
                bot: userinfo.value.usuario.bot? 1:0,
                image: userinfo.value.usuario.image.includes('base64') ? {
                    extension: 'png',
                    data: userinfo.value.usuario.image.split(',')[1]
                } : false
            })
            .then(() => {
                getData()
                newpass.value = ''
                $q.notify({
                    position: 'bottom-right',
                    type: 'positive',
                    message: 'Cambios Guardados'
                })
            }).catch(() => $store.commit('usuario/setLoading', false))
        }

        getData()

        return {
            setPicture,
            editarUsuario,
            userinfo,
            newpass,
            takepic: ref(false)
        }
    }

}
</script>

<style>

</style>