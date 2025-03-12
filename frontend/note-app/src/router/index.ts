import { createRouter, createWebHistory } from 'vue-router'
import type { RouteRecordRaw, NavigationGuardNext, RouteLocationNormalized } from 'vue-router'
import NotesLayout from '@/views/NotesLayout.vue'
import { useAuthStore } from '@/stores/authStore'

const HOME_ROUTE: RouteRecordRaw = {
  path: '/',
  redirect: '/notes'
}

const NOTE_CHILDREN_ROUTES: RouteRecordRaw[] = [
  {
    path: '',
    name: 'notes',
    component: () => import('@/views/NoteWelcome.vue')
  },
  {
    path: ':id',
    name: 'note-detail',
    component: () => import('@/components/notes/NotePanel.vue'),
    props: route => ({
      id: Number(route.params.id),
      editMode: false
    })
  },
  {
    path: ':id/edit',
    name: 'edit-note',
    component: () => import('@/components/notes/NotePanel.vue'),
    props: route => ({
      id: Number(route.params.id),
      editMode: true
    })
  },
  {
    path: 'new',
    name: 'new-note',
    component: () => import('@/components/notes/NotePanel.vue'),
    props: { isNew: true }
  }
]

const NOTES_ROUTE: RouteRecordRaw = {
  path: '/notes',
  component: NotesLayout,
  meta: { requiresAuth: true },
  children: NOTE_CHILDREN_ROUTES
}

const LOGIN_ROUTE: RouteRecordRaw = {
  path: '/login',
  name: 'login',
  meta: { guest: true },
  component: () => import('@/views/LoginView.vue')
}

const REGISTER_ROUTE: RouteRecordRaw = {
  path: '/register',
  name: 'register',
  meta: { guest: true },
  component: () => import('@/views/RegisterView.vue')
}

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    HOME_ROUTE,
    NOTES_ROUTE,
    LOGIN_ROUTE,
    REGISTER_ROUTE
  ],
})

const handleAuthNavigation = async (
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) => {
  const authStore = useAuthStore()
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth)
  const isGuestRoute = to.matched.some(record => record.meta.guest)
  
  if (authStore.token && !authStore.isLoading) await authStore.checkAuth()

  if (requiresAuth) {
    (authStore.isAuthenticated) ? next() : next({ name: 'login' });
  }
  else if (isGuestRoute) {
    (authStore.isAuthenticated) ? next({ path: '/notes' }) : next();
  }

  else {
    next()
  }
}

router.beforeEach(handleAuthNavigation)

export default router