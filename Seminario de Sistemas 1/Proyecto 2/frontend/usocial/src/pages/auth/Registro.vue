<template>
  <q-page class="flex flex-center bg-indigo-8" >
   <div class="column items-center">
      <div>
      <q-icon color="white" name="pages" size="150px" />
    </div>
    <q-card style="width: 450px">
      <q-card-section>
        <div class="text-h4 q-mb-md">Unirse a U-Social</div>
        <div class="row items-center justify-center " >
          <div style="width: 125px" class="relative-position">
            <img width="125" height="125" class="user-image" :src="picture.src" >
            <q-btn
              round
              color="positive"
              icon="add_a_photo"
              class="absolute-bottom-right"
              @click="takepic = true"
            />
          </div>
        </div>
        <div>
          <q-item >
            <q-item-section>
              <q-input  autofocus outlined color="indigo-10" v-model="nickname" type="text" label="Nombre de usuario" />
            </q-item-section>
          </q-item>          
        </div>
      </q-card-section>
      <q-separator  />
      <q-card-section>
        <div>
          <q-item>
            <q-item-section side>
              <q-icon name="person" color="indigo-10" size="md" />
            </q-item-section>
            <q-item-section>
              <q-input autofocus dense outlined color="indigo-10" v-model="username" type="text" label="Nombre(s) y Apellido(s)" />
            </q-item-section>
          </q-item>          
        </div>
        
        <div>
          <q-item>
            <q-item-section side>
              <q-icon name="email" color="indigo-10" size="md" />
            </q-item-section>
            <q-item-section>
              <q-input autofocus dense outlined color="indigo-10" v-model="mail" type="text" label="Correo electronico" />
            </q-item-section>
          </q-item>          
        </div>
        <div>
          <q-item>
            <q-item-section side>
              <q-icon name="lock" color="indigo-10" size="md" />
            </q-item-section>
            <q-item-section>
              <q-input autofocus dense outlined color="indigo-10" v-model="password" type="password" label="Contraseña" />
            </q-item-section>
          </q-item>          
        </div>

      </q-card-section>
      <q-separator spaced />
      <q-card-actions  align="between">
        <q-btn to="/signin" flat color="grey-6" label="Ingresar" />
        <q-btn  unelevated color="indigo-10" label="Registrar" @click="registrarUsuario" />
      </q-card-actions>
    </q-card>
   </div>
  </q-page>
   <q-dialog v-model="takepic" >
       <TomarFoto @picture="setPicture" />
   </q-dialog>
</template>

<script>
import { ref } from "vue";
import { useStore } from "vuex";
import { useQuasar } from "quasar"
import { useRouter } from "vue-router";

import TomarFoto from '../../components/TomarFoto.vue';

export default {
  components:{
    TomarFoto
  },
  setup(){
    const $store = useStore()
    const $q = useQuasar()
    const $router = useRouter()

    const username = ref('')
    const mail = ref('')
    const nickname = ref('')
    const password = ref('')
    const picture = ref({src: '', ext: ''})
    
    const resetValues = () => {
      username.value = '',
      mail.value = '',
      nickname.value = '',
      password.value = ''
    }

    const setPicture = (value) => {
      picture.value = value
    } 

    const registrarUsuario = () => {
      $q.loading.show({
        message: 'Procesando...',
        spinnerColor: 'white'
      })
      $store.dispatch('usuario/registrar', {
        name: username.value,
        email: mail.value,
        nickname: nickname.value,
        password: password.value,
        image: {
          extension: picture.value.ext,
          data: picture.value.src.split(',')[1]
        }
      }).then(data => {
        resetValues()
        $q.notify({
          message: 'Usuario registrado, esperando verificacion de email',
          position: 'bottom',
          timeout: 6000,
          type: 'positive'
        })
        $router.push('/signin')
      }).catch(errmsg => {
        $q.notify({
          message: errmsg,
          position: 'bottom',
          timeout: 6000,
          type: 'negative',
          color: 'red'
        })
        resetValues()
      }).finally(() => $q.loading.hide())
    }

    return {
      setPicture,
      registrarUsuario,
      nickname,
      password,
      picture,
      mail,
      username,
      takepic: ref(false)
    }
  }

}
</script>

<style scoped>
.user-image {
  border-radius: 100%;
  border: 1px solid rgb(201, 201, 201);
}
</style>