import { defineConfig } from 'vitepress'

export default defineConfig({
  base: '/MassaK.Plugin/',
  title: "MassaK plugin",
  description: "A documentation for nuget package",
  themeConfig: {
    nav: [
      { text: 'Home', link: '/' },
      { text: 'Guide', link: '/guide/' }
    ],
    sidebar: [
      {
        text: 'Documentation',
        items: [
          { text: 'Installation', link: '/guide/index.md' },
          { text: 'How to use', link: '/guide/usage.md' },
          { text: 'Events', link: '/guide/events.md' }
        ]
      },
      {
        text: 'Examples',
        items: [
          { text: 'Blazor server', link: '/guide/blazor-server-example.md' },
        ]
      }
    ],
    socialLinks: [
      { icon: 'github', link: 'https://github.com/VladStandard/MassaK.Plugin' }
    ]
  }
})
