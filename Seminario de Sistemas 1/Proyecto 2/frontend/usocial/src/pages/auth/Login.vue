<template>
  <q-page class="flex flex-center bg-indigo-8" >
   <div class="column items-center">
      <div>
      <q-icon color="white" name="pages" size="150px" />
    </div>
    <q-card >
      <q-card-section>
        <div class="text-h4">Iniciar Sesion </div>
      </q-card-section>
      <q-separator spaced />
      <q-card-section>
        <div>
          <q-item>
            <q-item-section side>
              <q-icon name="account_circle" color="indigo-10" size="md" />
            </q-item-section>
            <q-item-section>
              <q-input autofocus dense outlined color="indigo-10" v-model="nickname" type="text" label="Nombre de usuario" />
            </q-item-section>
          </q-item>          
        </div>
        <div>
          <div class="q-mt-md" v-if="picture.src" >
            <div>
              <img class="rounded-borders" :src="picture.src" width="350" />
            </div>
            <div>
              <q-btn color="red" label="Usar Contraseña" flat unelevated @click="picture.src = ''"  />
            </div>
          </div>
          <q-item v-else>
            <q-item-section side>
              <q-icon name="lock" color="indigo-10" size="md" />
            </q-item-section>
            <q-item-section>
              <q-input  dense outlined color="indigo-10" v-model="password" type="password" label="Contraseña" />
            </q-item-section>
            <q-item-section side >
              <q-btn icon="center_focus_weak" color="green-6" round @click="takepic = true" unelevated/>
            </q-item-section>
          </q-item>
        </div>
      </q-card-section>
      <q-separator spaced />
      <q-card-actions  align="between">
        <q-btn to="/signup" flat color="grey-6" label="Registrar" />
        <q-btn @click="autenticar"  unelevated color="indigo-10" label="Ingresar" />
      </q-card-actions>
    </q-card>
   </div>
  </q-page>
   <q-dialog v-model="takepic" >
       <TomarFoto @picture="setPicture" />
   </q-dialog>
</template>

<script>
import { useStore } from "vuex";
import { useRouter } from 'vue-router'
import { useQuasar } from 'quasar'
import { ref } from "vue";

import TomarFoto from '../../components/TomarFoto.vue';

export default {
  components: { TomarFoto },
  setup() {
    const $store = useStore()
    const $router = useRouter()
    const $q = useQuasar()

    const nickname = ref('')
    const password = ref('')
    const picture = ref({src: '', ext:''})

    const setPicture = (value) => {
      picture.value = value
    } 

    const autenticar = () => {
      let payload = {
        nickname: nickname.value
      }
      let method = 'rekognition'
      if (picture.value.src) {
        payload.image = {
          data: picture.value.src.split(',')[1]
        }
      } else {
        method = 'password'
        payload.password = password.value
      }

      $q.loading.show({
        message: 'Procesando...',
        spinnerColor: 'white'
      })
      $store.dispatch(`usuario/${method}`, payload)
      .then(data => {
        $router.push('/u-social')
      }).catch(errmsg => {
        console.log(errmsg);
        $q.notify({
          message: errmsg,
          position: 'bottom',
          timeout: 6000,
          type: 'negative',
          color: 'red'
        })
      }).finally(() => $q.loading.hide())
    }

    return {
      setPicture,
      autenticar,
      nickname,
      password,
      picture,
      takepic: ref(false)
    }
  }

}
</script>

<style>

</style>