<template>
    <q-card dark style="width: 500px">
        <q-card-section>
            <q-item>
                <q-item-section top avatar>
                    <q-avatar color="primary" text-color="white" icon="sick" />
                </q-item-section>
                <q-item-section>
                    <q-item-label>Covid-Chan</q-item-label>
                </q-item-section>
            </q-item>
        </q-card-section>
        <q-separator spaced />
        <q-card-section>
            <div v-for="(msg, index) in sequence" :key="index">
                <q-chat-message
                    :name="msg.user"
                    :text="[msg.msg]"
                    :sent="msg.user === store.state.usuario.nickname? true : false"
                />              
            </div>
            <div class="row items-center justify-center">
                <q-input dark @keypress.enter="sendMessage" class="q-mx-sm col-grow" outlined dense v-model="newmsg" type="text" label="Aa" />
                <q-btn class="q-mx-sm" color="white" icon="send" round unelevated flat @click="sendMessage" />
            </div>
        </q-card-section>
    </q-card>
</template>

<script>

import { ref } from 'vue'
import { useStore } from "vuex";

export default {
    setup(){
        const store = useStore()

        const newmsg = ref('')

        const sequence = ref([])

        const sendMessage = () => {
            sequence.value.push({
                user: store.state.usuario.nickname,
                msg: newmsg.value
            })
            store.dispatch('usuario/covid', newmsg.value)
            .then(values => {
                for (let index = 1; index <= values.length; index++) {
                    const element = values[index - 1];
                    setTimeout(() => {
                        sequence.value.push({
                            user: 'covid-chan',
                            msg: element
                        })
                    }, 1000 * index);
                }
            })
            .catch(() => {
                setTimeout(() => {
                    sequence.value.push({
                        user: 'covid-chan',
                        msg: 'Parece que por ahora no podre ayudarte :c'
                    })
                }, 1000);
            })
            newmsg.value = ''
            
        }
        sequence.value.push({
            user: 'covid-chan',
            msg: '¡HOLA! soy tu informante oficial sobre Covid-19'
        })

        setTimeout(() => {
            sequence.value.push({
                user: 'covid-chan',
                msg: 'Escribe el nombre de un pais y yo te respondere con la informacion correspondiente'
            }) 
        }, 1000);

        return {
            newmsg,
            sequence,
            store,
            sendMessage
        }
    }

}
</script>

<style>

</style>