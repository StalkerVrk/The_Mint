from telegram import Bot, Update
from telegram.ext import Application, CommandHandler, CallbackQueryHandler, InlineQueryHandler,ContextTypes, MessageHandler

import os

# Токен бота
TOKEN = "7808808108:AAEUYz4B0GOsiA5UDo9io-3YYgj2m2rk6vQ"

# Имя игры
GAME_NAME = "SpaceClick"

# Путь к статическим файлам (предполагается, что BuildUnity - это папка с статическими файлами)
STATIC_PATH = os.path.join(os.path.dirname(__file__), './BuildUnity')

# Создаем экземпляр бота
bot = Bot(token=TOKEN)

# Создаем Application с использованием бота
application = Application.builder().token(TOKEN).build()

# Словарь для хранения информации о запросах
queries = {}

# Функция для обработки команды /help
async def help_command(update: Update, context: ContextTypes.DEFAULT_TYPE):
    await update.message.reply_text("Say /game if you want to play.")

# Функция для обработки команды /start или /game
async def start_game(update: Update, context: ContextTypes.DEFAULT_TYPE):
    await update.message.reply_game(GAME_NAME)

# Функция для обработки inline-запросов
async def inline_query(update: Update, context: ContextTypes.DEFAULT_TYPE):
    return await update.callback_query.answer(
        [
            {
                "type": "game",
                "id": "0",
                "game_short_name": GAME_NAME
            }
        ]
    )

# Функция для обработки callback-queries
async def callback_query_handler(update: Update, context: ContextTypes.DEFAULT_TYPE):
    if update.callback_query.game_short_name != f"{GAME_NAME}":
        await update.callback_query.answer(f"Sorry, '{update.callback_query.game_short_name}' is not available.")
    else:
        queries[update.callback_query.id] = update.callback_query
        game_url = "https://stalkervrk.github.io/SpaceClick/"
        await update.callback_query.answer(url=game_url)

# Функция для обновления счета игрока
async def set_highscore(update: Update, context: ContextTypes.DEFAULT_TYPE):
    query_id = update.callback_query.id
    if query_id in queries:
        query = queries[query_id]
        
        options = {}
        if hasattr(query, 'message'):
            options = {
                'chat_id': query.message.chat.id,
                'message_id': query.message.message_id
            }
        elif hasattr(query, 'inline_message_id'):
            options = {'inline_message_id': query.inline_message_id}
        
        await bot.set_game_score(
            chat_id=query.from_user.id,
            score=int(context.args[0]),
            options=options,
            callback=lambda err, result: None
        )

# Настройка обработчиков команд
application.add_handler(CommandHandler("help", help_command))
application.add_handler(CommandHandler(["start", "game"], start_game))
application.add_handler(InlineQueryHandler(inline_query))
application.add_handler(CallbackQueryHandler(callback_query_handler))

# Запуск бота
application.run_polling()