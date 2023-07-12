
const routes = [
  {
    path: '/',
    redirect: '/signin',
    component: () => import('layouts/BaseLayout.vue'),
    children: [
      { path: 'signin', component: () => import('pages/auth/Login.vue') },
      { path: 'signup', component: () => import('pages/auth/Registro.vue')}
    ]
  },
  {
    path: '/u-social',
    component: () => import('layouts/MainLayout.vue')
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/Error404.vue')
  }
]

export default routes
